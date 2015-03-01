using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataStore
{
    internal sealed class KeyProvider
    {
        internal KeyProvider(string keysDirectory)
        {
            if (!Directory.Exists(keysDirectory))
            {
                Directory.CreateDirectory(keysDirectory);
                tagKeyProviders = new ConcurrentDictionary<string, TagKeys>();
            }
            else
            {
                var existingKeys = Directory.EnumerateFiles(keysDirectory)
                    .Select(tagKeysFile => new KeyValuePair<string,TagKeys>(
                        Path.GetFileNameWithoutExtension(tagKeysFile),
                        new TagKeys(tagKeysFile)))
                    .ToList();

                tagKeyProviders = new ConcurrentDictionary<string, TagKeys>(existingKeys);
            }

            this.keysDirectory = keysDirectory;
        }

        private string keysDirectory;

        private ConcurrentDictionary<string, TagKeys> tagKeyProviders;

        public long GetKey(string tag)
        {
            var tagIDProvider = tagKeyProviders.GetOrAdd(tag,
                (newTag) =>
                {
                    var tagFile = keysDirectory + newTag + ".txt";
                    return new TagKeys(tagFile);
                });

            return tagIDProvider.GetNewKey();
        }
    }

    class TagKeys
    {
        public TagKeys(string tagFile)
        {
            if (!File.Exists(tagFile))
            {
                File.WriteAllText(tagFile,"0");
            }
            else
            {
                var tagTxt = File.ReadAllText(tagFile);
                lastKey = Int64.Parse(tagTxt);
            }

            this.tagFile = tagFile;
        }

        long lastKey = 0;
        string tagFile;
        object syncRoot = new object();

        public long GetNewKey()
        {
            // TODO 1 Create task loop class, so ID's are returned instantly,
            // but saving new ID's are done concurrently, and a count of all
            // new global ID's generated are saved, so that inexpected shutdown
            // increments all new last known ID's to maintain consistency.
            //return Interlocked.Increment(ref lastID);

            lock(syncRoot)
            {
                lastKey += 1;
                File.WriteAllText(tagFile, lastKey.ToString());
                return lastKey;
            }
        }
    }
}
