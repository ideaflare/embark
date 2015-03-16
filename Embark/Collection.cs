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
    public class Collection
    {
        public Collection(string tag, ITextDataStore repository)
        {
            this.tag = tag;
            this.channel = repository;
        }

        string tag;
        ITextDataStore channel;

        public long Insert<T>(T objectToInsert) where T : class
        {
            string jsonText = JsonConvert.SerializeObject(objectToInsert, Formatting.Indented);
            return channel.Insert(tag, jsonText);
        }

        public T Select<T>(long id) where T : class
        {
            var jsonText = channel.Select(tag, id);

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
            string jsonTextObject = JsonConvert.SerializeObject(searchObject, Formatting.Indented);

            var jsonTextEnumerable = channel.SelectLike(tag, jsonTextObject);
            foreach (var jsonText in jsonTextEnumerable)
            {
                yield return JsonConvert.DeserializeObject<T>(jsonText);
            }
        }

        public int UpdateLike(object startRange, object endRange)
        {
            throw new NotImplementedException();
        }

        public int DeleteLike(object searchObject)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> SelectBetween<T>(object startRange, object endRange) where T : class
        {
            throw new NotImplementedException();
        }

        public int UpdateBetween(object startRange, object endRange, object newValue)
        {
            throw new NotImplementedException();
        }

        public int DeleteBetween(object startRange, object endRange)
        {
            throw new NotImplementedException();
        }
    }
}
