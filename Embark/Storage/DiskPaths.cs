using System.IO;
using System.Collections.Concurrent;

namespace Embark.Storage
{
    internal class DiskPaths
    {
        public DiskPaths(string collectionDirectory)
        {
            if (!Directory.Exists(collectionDirectory))
                Directory.CreateDirectory(collectionDirectory);

            CollectionDirectory = collectionDirectory;
        }

        internal string CollectionDirectory { get; }
        private ConcurrentDictionary<string, string> tagPathLookup = new ConcurrentDictionary<string, string>();

        public string GetCollectionDirectory(string tag)
            => tagPathLookup.GetOrAdd(tag, newSeenTag =>
            {
                string tagDir = CollectionDirectory + newSeenTag + "\\";
                if (!Directory.Exists(tagDir))
                    Directory.CreateDirectory(tagDir);
                return tagDir;
            });

        public string GetDocumentPath(string tag, long key)
        {
            var tagDir = GetCollectionDirectory(tag);
            var savePath = tagDir + key + ".txt";
            return savePath;
        }
    }
}
