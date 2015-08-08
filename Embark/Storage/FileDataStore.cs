using System.IO;
using System.Linq;
using Embark.DataChannel;
using System.Collections.Generic;

namespace Embark.Storage
{
    internal class FileDataStore : IDataStore
    {
        public FileDataStore(string directory)
        {
            var appendBackslash = directory.EndsWith("\\") ? "" : "\\";

            var collectionsFolder = directory + appendBackslash + @"Collections\";

            tagPaths = new CollectionPaths(collectionsFolder);
        }
        
        private CollectionPaths tagPaths;

        public void Insert(string tag, long key, string objectToInsert)
        {
            var savePath = tagPaths.GetDocumentPath(tag, key);

            File.WriteAllText(savePath, objectToInsert);
        }
        
        public bool Update(string tag, long id, string objectToUpdate)
        {
            var savePath = tagPaths.GetDocumentPath(tag, id);

            if (File.Exists(savePath))
            {
                File.WriteAllText(savePath, objectToUpdate);
                return true;
            }
            else return false;
        }

        public bool Delete(string tag, long id)
        {
            var savePath = tagPaths.GetDocumentPath(tag, id);

            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                return true;
            }
            else return false;
        }

        public string Get(string tag, long id)
        {
            var savePath = tagPaths.GetDocumentPath(tag, id);

            return File.Exists(savePath) ?
                File.ReadAllText(savePath) : null;
        }

        public IEnumerable<string> Collections => Directory
            .EnumerateDirectories(tagPaths.CollectionDirectory)
            .Select(path => new DirectoryInfo(path).Name);

        public DataEnvelope[] GetAll(string tag)
        {
            var tagDir = tagPaths.GetCollectionDirectory(tag);

            return GetEnvelopes(tagDir);
        }

        private DataEnvelope[] GetEnvelopes(string directory)
            =>
            Directory.GetFiles(directory)
            .Select(filePath => GetDataEnvelope(filePath))
            .Where(envelope => envelope.ID > 0)
            .ToArray();

        // TODO Try/Catch return error envelope.
        private DataEnvelope GetDataEnvelope(string filePath)        
            => File.Exists(filePath) ?
            new DataEnvelope
            {
                ID = long.Parse(Path.GetFileNameWithoutExtension(filePath)),
                Text = File.ReadAllText(filePath)
            }
            : new DataEnvelope { ID = -1 };
    }
}
