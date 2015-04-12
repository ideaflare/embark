using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Embark.Interfaces;
using Embark.Conversion;
using System.ServiceModel;

namespace Embark.Storage
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    internal class TextFileRepository : ITextDataStore
    {
        public TextFileRepository(string directory, ITextConverter textComparer)
        {
            if (!directory.EndsWith("\\"))
                directory += "\\";

            var collectionsFolder = directory + @"Collections\";
            var keysFolder = directory + @"Map\";
            
            this.keyProvider = new KeyProvider(keysFolder);
            this.tagPaths = new CollectionPaths(collectionsFolder);

            this.textComparer = textComparer;
        }
        
        private KeyProvider keyProvider;
        private CollectionPaths tagPaths;
        private object syncRoot = new object();

        private ITextConverter textComparer;

        // Basic
        long ITextDataStore.Insert(string tag, string objectToInsert)
        {
            // Get ID from IDGen
            var key = keyProvider.GetNewKey();
                
            // TODO 3 offload to queue that gets processed by task
            var savePath = tagPaths.GetDocumentPath(tag, key.ToString());

            // TODO 1 NB get a document only lock, instead of all repositories lock
            lock (syncRoot)
            {
                // Save object to tag dir
                File.WriteAllText(savePath, objectToInsert);

                //Return ID to client
                return key;
            }
        }
        
        bool ITextDataStore.Update(string tag, string id, string objectToUpdate)
        {
            var savePath = tagPaths.GetDocumentPath(tag, id);
            
            lock(syncRoot)
            {
                if (!File.Exists(savePath))
                    return false;
                else
                {
                    File.WriteAllText(savePath, objectToUpdate);
                    return true;
                }
            }
        }

        bool ITextDataStore.Delete(string tag, string id)
        {
            var savePath = tagPaths.GetDocumentPath(tag, id);

            lock (syncRoot)
            {
                if (File.Exists(savePath))
                {
                    File.Delete(savePath);
                    return true;
                }
                else return false;
            }
        }

        string ITextDataStore.Select(string tag, string id)
        {
            var savePath = tagPaths.GetDocumentPath(tag, id);

            string jsonText;
            // TODO lock row only
            lock (syncRoot)
            {
                if (!File.Exists(savePath))
                    return null;

                jsonText = File.ReadAllText(savePath);
            }
            return jsonText;
        }

        IEnumerable<DataEnvelope> ITextDataStore.SelectAll(string tag)
        {
            lock(syncRoot)
            {
                var allFiles = GetAllTagFiles(tag);

                return allFiles;
            }
        }

        IEnumerable<DataEnvelope> ITextDataStore.SelectLike(string tag, string searchObject)
        {
            lock (syncRoot)
            {
                var allFiles = GetAllTagFiles(tag);

                var propertyLookup = textComparer.ToComparisonObject(searchObject);

                var matches = allFiles
                    .Select(envelope => new
                    {
                        envelope = envelope,
                        graph = textComparer.ToComparisonObject(envelope.Text)
                    })
                    .Where(comparison => textComparer.IsMatch(propertyLookup, comparison.graph))
                    .Select(e => e.envelope);

                return matches;
            }
        }
        
        IEnumerable<DataEnvelope> ITextDataStore.SelectBetween(string tag, string startRange, string endRange)
        {
            lock (syncRoot)
            {
                var allFiles = GetAllTagFiles(tag);

                var startLookup = textComparer.ToComparisonObject(startRange);
                var endLookup = textComparer.ToComparisonObject(endRange);

                var matches = allFiles
                   .Select(envelope => new
                   {
                       envelope = envelope,
                       graph = textComparer.ToComparisonObject(envelope.Text)
                   })
                   .Where(comparison => textComparer.IsBetweenMatch(startLookup, endLookup, comparison.graph))
                   .Select(e => e.envelope);

                return matches;
            }
        }

        private IEnumerable<DataEnvelope> GetAllTagFiles(string tag)
        {
            var tagDir = tagPaths.GetCollectionDirectory(tag);

            var allFiles = Directory
                .EnumerateFiles(tagDir)
                .Select(filePath => GetDataEnvelope(filePath));

            return allFiles;
        }

        // TODO Try/Catch return error envelope.
        private DataEnvelope GetDataEnvelope(string filePath)
        {
            return new DataEnvelope
            {
                ID = Int64.Parse(Path.GetFileNameWithoutExtension(filePath)),
                Text = File.ReadAllText(filePath)
            };
        }
    }
}
