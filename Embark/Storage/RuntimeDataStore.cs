using System;
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

        bool IDataStore.Delete(string tag, long id)
        {
            string na;
            return collections[tag]?.TryRemove(id, out na) ?? false;
        }

        string IDataStore.Get(string tag, long id) 
            => collections[tag]?[id];

        DataEnvelope[] IDataStore.GetAll(string tag)
            => collections[tag]
            .Select(kv => new DataEnvelope
            {
                ID = kv.Key,
                Text = kv.Value
            }).ToArray();

        void IDataStore.Insert(string tag, long key, string objectToInsert)
        {
            collections.GetOrAdd(tag, new ConcurrentDictionary<long, string>())[key] = objectToInsert;
        }

        bool IDataStore.Update(string tag, long id, string objectToUpdate)
        {
            if (collections[tag].ContainsKey(id))
            {
                collections[tag][id] = objectToUpdate;
                return true;
            }
            else return false;
        }
    }
}
