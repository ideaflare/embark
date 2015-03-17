using Embark.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Conversion
{
    public class Collection
    {
        public Collection(string tag, ITextDataStore textDataStore, ITextConverter textConverter)
        {
            this.tag = tag;
            this.textDataStore = textDataStore;
            this.textConverter = textConverter;
        }

        string tag;
        ITextDataStore textDataStore;
        ITextConverter textConverter;

        public long Insert<T>(T objectToInsert) where T : class
        {   
            string text = textConverter.ToText(objectToInsert);

            return textDataStore.Insert(tag, text);
        }

        public T Select<T>(long id) where T : class
        {
            var text = textDataStore.Select(tag, id);

            return text == null ? null :
                textConverter.ToObject<T>(text);
        }

        public bool Update(long id, object objectToUpdate)
        {
            string text = textConverter.ToText(objectToUpdate);
            return textDataStore.Update(tag, id, text);
        }

        public bool Delete(long id)
        {
            return textDataStore.Delete(tag, id);
        }

        public IEnumerable<T> SelectLike<T>(object searchObject)
            where T : class
        {
            string searchText = textConverter.ToText(searchObject);

            var searchResults = textDataStore.SelectLike(tag, searchText);

            foreach (var result in searchResults)
            {
                yield return textConverter.ToObject<T>(result);
            }
        }

        public int UpdateLike(object startRange, object endRange)
        {
            throw new NotImplementedException();
        }

        public int DeleteLike(object searchObject)
        {
            string deleteSearch = textConverter.ToText(searchObject);

            return textDataStore.DeleteLike(tag, deleteSearch);
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
