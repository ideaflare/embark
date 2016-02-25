using System.Linq;
using System.Collections.Generic;
using Embark.DataChannel;
using System.Collections.Concurrent;

namespace Embark.Storage
{
    class RuntimeDataStore : IDataStore
    {
        private ConcurrentDictionary<string, ConcurrentDictionary<long, string>> collections
            = new ConcurrentDictionary<string, ConcurrentDictionary<long, string>>();

        IEnumerable<string> IDataStore.Collections => collections.Keys;
                            
        void IDataStore.Insert(string tag, long id, string objectToInsert)
        {
            var tagCollection = collections.GetOrAdd(tag, new ConcurrentDictionary<long, string>());
            tagCollection[id] = objectToInsert;
        }

        bool IDataStore.Update(string tag, long id, string objectToUpdate)
        {
            if(this[tag]?.ContainsKey(id) ?? false)
            {
                this[tag].AddOrUpdate(id, objectToUpdate, (k, v) => objectToUpdate);
                return true;
            }
            else return false;
        }

        bool IDataStore.Delete(string tag, long id)
        {
            string na;
            return this[tag]?.TryRemove(id, out na) ?? false;
        }

        string IDataStore.Get(string tag, long id) => GetValueOrNull(id, this[tag]);

        ConcurrentDictionary<long, string> this[string tag] => GetValueOrNull(tag, collections);

        DataEnvelope[] IDataStore.GetAll(string tag)
            => this[tag]?.Select(kv => new DataEnvelope(kv.Key, kv.Value)).ToArray()
            ?? new DataEnvelope[] { };

        private T GetValueOrNull<Key, T>(Key key, ConcurrentDictionary<Key, T> dictionary) where T : class
        {
            T tVal = null;
            dictionary?.TryGetValue(key, out tVal);
            return tVal;
        }
    }
}
