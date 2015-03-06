using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Embark.Cache
{
    internal sealed class KeyProvider
    {
        internal KeyProvider(string keysDirectory)
        {
            if (!Directory.Exists(keysDirectory))            
                Directory.CreateDirectory(keysDirectory);
            
            keysFile = keysDirectory + "LatestKey.txt";

            if (!File.Exists(keysFile))            
                File.WriteAllText(keysFile,"0");            
            else
            {
                var keyTxt = File.ReadAllText(keysFile);
                lastKey = Int64.Parse(keyTxt);
            }
        }

        long lastKey = 0;

        string keysFile;
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
                var newKey = DateTime.Now.Ticks;
                while (newKey <= lastKey)
                    newKey++;
                lastKey = newKey;

                File.WriteAllText(keysFile, lastKey.ToString());
                return lastKey;
            }
        }
    }
}
