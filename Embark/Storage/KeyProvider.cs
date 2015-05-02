using System;
using System.IO;

namespace Embark.Storage
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
            // TODO 1 Append to logfile to return faster
            // 2nd task runs that empties log(s) to text file.
            // Write generically so that log writer/comitter(s)
            // can be re-used for collection insert/update/delete commands also

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
