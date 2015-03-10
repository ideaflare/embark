using Embark.Cache;
using Embark.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark
{
    public class Collection : IDataStore
    {
        public Collection(string tag, Repository repository)
        {
            this.tag = tag;
            this.channel = repository;
        }

        string tag;
        Repository channel;

        public long Insert<T>(T objectToInsert) where T : class
        {
            string jsonText = JsonConvert.SerializeObject(objectToInsert, Formatting.Indented);
            return channel.Insert(tag, jsonText);
        }

        public T Select<T>(long id) where T : class
        {
            var jsonText = channel.Get(tag, id);

            return jsonText == null ? null :
                JsonConvert.DeserializeObject<T>(jsonText);
        }

        public bool Update(long id, object objectToUpdate)
        {
            string jsonText = JsonConvert.SerializeObject(objectToUpdate, Formatting.Indented);
            return channel.Update(tag, id, jsonText);
        }

        public bool Delete(long id)
        {
            return channel.Delete(tag, id);
        }

        public IEnumerable<T> SelectLike<T>(Object searchObject)
            where T : class
        {
            var jsonTextEnumerable = channel.GetWhere(tag, searchObject);
            foreach (var jsonText in jsonTextEnumerable)
            {
                yield return JsonConvert.DeserializeObject<T>(jsonText);
            }
        }

        public IEnumerable<T> SelectBetween<T>(object searchObject, object optionalEndrange) where T : class
        {
            throw new NotImplementedException();
        }

        public int UpdateLike(object searchObject, object newValue)
        {
            throw new NotImplementedException();
        }

        public int UpdateBetween(object searchObject, object endrange, object newValue)
        {
            throw new NotImplementedException();
        }

        public int DeleteLike(object searchObject)
        {
            throw new NotImplementedException();
        }

        public int DeleteBetween(object searchObject, object optionalEndrange)
        {
            throw new NotImplementedException();
        }
    }
}
