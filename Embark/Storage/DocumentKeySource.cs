using System;

namespace Embark.Storage
{
    internal sealed class DocumentKeySource
    {
        internal DocumentKeySource(long lastKey)
        {
            this.lastKey = lastKey;
        }

        long lastKey = 0;

        object syncRoot = new object();

        public long GetNewKey()
        {
            var newKey = DateTime.Now.Ticks;

            lock(syncRoot)
            {
                if (newKey > lastKey)
                    lastKey = newKey;
                else lastKey += 1;

                return lastKey;
            }
        }
    }
}
