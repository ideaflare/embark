using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Embark.Interfaces;
using Embark.Conversion;

namespace Embark.Storage
{
    public class TextFileRepository : ITextDataStore
    {
        public TextFileRepository(string directory)
        {
            if (!directory.EndsWith("\\"))
                directory += "\\";

            var collectionsFolder = directory + @"Collections\";
            var keysFolder = directory + @"Map\";
            
            this.keyProvider = new KeyProvider(keysFolder);
            this.tagPaths = new CollectionPaths(collectionsFolder);
        }
        
        private KeyProvider keyProvider;
        private CollectionPaths tagPaths;
        private object syncRoot = new object();

        // Basic
        long ITextDataStore.Insert(string tag, string objectToInsert)
        {
            // Get ID from IDGen
            var key = keyProvider.GetNewKey();
                
            // TODO 3 offload to queue that gets processed by task
            var savePath = tagPaths.GetDocumentPath(tag, key);

            // TODO 1 NB get a document only lock, instead of all repositories lock
            lock (syncRoot)
            {
                // Save object to tag dir
                File.WriteAllText(savePath, objectToInsert);

                //Return ID to client
                return key;
            }
        }
        
        string ITextDataStore.Select(string tag, long id)
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

        bool ITextDataStore.Update(string tag, long id, string objectToUpdate)
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

        bool ITextDataStore.Delete(string tag, long id)
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

        IEnumerable<string> ITextDataStore.SelectLike(string tag, string searchObject)
        {
            lock(syncRoot)
            {
                var tagDir = tagPaths.GetCollectionDirectory(tag);
                var allItems = Directory
                    .GetFiles(tagDir)
                    .Select(f => File.ReadAllText(f))
                    .ToList();

                return Comparison.GetLikeMatches(searchObject, allItems);                
            }
        }               
        
        int ITextDataStore.UpdateLike(string tag, string searchObject, string newValue)
        {
            throw new NotImplementedException();
        }

        int ITextDataStore.DeleteLike(string tag, string searchObject)
        {
            throw new NotImplementedException();
        }

        IEnumerable<string> ITextDataStore.SelectBetween(string tag, string startRange, string endRange)
        {
            throw new NotImplementedException();
        }

        int ITextDataStore.UpdateBetween(string tag, string startRange, string endRange, string newValue)
        {
            throw new NotImplementedException();
        }

        int ITextDataStore.DeleteBetween(string tag, string startRange, string endRange)
        {
            throw new NotImplementedException();
        }
    }
}
