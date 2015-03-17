using Embark.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Conversion
{
    public class Collection
    {
        public Collection(string tag, ITextDataStore textDataStore)
        {
            this.tag = tag;
            this.textDataStore = textDataStore;
        }

        string tag;
        ITextDataStore textDataStore;

        public long Insert<T>(T objectToInsert) where T : class
        {
            var lizer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var JTx = lizer.Serialize(objectToInsert);

            string jsonText = JsonConvert.SerializeObject(objectToInsert, Formatting.Indented);
            return textDataStore.Insert(tag, jsonText);
        }

        public T Select<T>(long id) where T : class
        {
            var jsonText = textDataStore.Select(tag, id);

            return jsonText == null ? null :
                JsonConvert.DeserializeObject<T>(jsonText);
        }

        public bool Update(long id, object objectToUpdate)
        {
            string jsonText = JsonConvert.SerializeObject(objectToUpdate, Formatting.Indented);
            return textDataStore.Update(tag, id, jsonText);
        }

        public bool Delete(long id)
        {
            return textDataStore.Delete(tag, id);
        }

        public IEnumerable<T> SelectLike<T>(Object searchObject)
            where T : class
        {
            string jsonTextObject = JsonConvert.SerializeObject(searchObject, Formatting.Indented);

            var jsonTextEnumerable = textDataStore.SelectLike(tag, jsonTextObject);
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
