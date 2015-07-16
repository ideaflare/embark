using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Interaction.Concurrency
{
    /// <summary>
    /// List of objects to lock on hashcodes
    /// <para>
    /// for example: lock(hashlock.GetLock("cat")) instead of unsafe lock("cat")
    /// </para>
    /// </summary>
    public class HashLock
    {
        /// <summary>
        /// Create a new instance with a dictionary of objects for locking
        /// </summary>
        /// <param name="avaliableLocks">The maximum number of shared locks</param>
        public HashLock(int avaliableLocks)
        {
            locks = new ConcurrentDictionary<int, object>(
                Enumerable.Range(0, avaliableLocks)
                .Select(i => new KeyValuePair<int, object>(i, new object())));
        }

        private ConcurrentDictionary<int, object> locks;

        /// <summary>
        /// Get an object to lock on based on the hash of another object
        /// <para>
        /// NOTE: NB do not modify the object returned.
        /// </para>
        /// </summary>
        /// <param name="obj">Object to call .GetHashCode() on to get a lock</param>
        /// <returns>Object to use for locking</returns>
        public object GetLock(object obj)
        {
            if (obj == null)
                return locks[0];

            var index = Math.Abs(obj.GetHashCode()) % locks.Count;

            return locks[index];
        }
    }
}
