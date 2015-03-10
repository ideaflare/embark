using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Interfaces
{
    /// <summary>
    /// All commands to insert/get/update/delete documents
    /// </summary>
    public interface IDataStore
    {
        // Basic

        /// <summary>
        /// Insert a json serializable object to save to a collection
        /// </summary>
        /// <typeparam name="T">Any json serializable/deserializable object</typeparam>
        /// <param name="tag">A name of the collection to save it in</param>
        /// <param name="objectToInsert">Anything json serializable/deserializable object to save</param>
        /// <returns>An Int64 ID unique to the collection the object is saved in</returns>
        long Insert<T>(T objectToInsert) 
            where T : class;

        T Select<T>(long id) 
            where T : class;

        bool Update(long id, object objectToUpdate);
        bool Delete(long id);

        // Range
        IEnumerable<T> SelectLike<T>(object searchObject)
            where T : class;

        IEnumerable<T> SelectBetween<T>(object searchObject, object optionalEndrange) 
            where T : class;

        int UpdateLike(object searchObject, object newValue);
        int UpdateBetween(object searchObject, object endrange, object newValue);

        int DeleteLike(object searchObject);
        int DeleteBetween(object searchObject, object optionalEndrange);
    }
}
