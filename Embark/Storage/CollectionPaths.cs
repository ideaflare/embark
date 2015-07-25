using System.IO;
using System.Collections.Concurrent;

namespace Embark.Storage
{
    internal class CollectionPaths
    {
        public CollectionPaths(string collectionDirectory)
        {
            if (!Directory.Exists(collectionDirectory))
                Directory.CreateDirectory(collectionDirectory);

            this.collectionDirectory = collectionDirectory;
        }

        private string collectionDirectory;
        private static ConcurrentDictionary<string, string> tagPathLookup = new ConcurrentDictionary<string, string>();

        public string GetCollectionDirectory(string tag)
            => tagPathLookup.GetOrAdd(tag, newSeenTag =>
            {
                string tagDir = collectionDirectory + newSeenTag + "\\";
                if (!Directory.Exists(tagDir))
                    Directory.CreateDirectory(tagDir);
                return tagDir;
            });

        public string GetDocumentPath(string tag, string key)
        {
            var tagDir = GetCollectionDirectory(tag);
            var savePath = tagDir + key + ".txt";
            return savePath;
        }
    }
}
