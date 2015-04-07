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
    internal interface ICollection
    {
        // Basic

        /// <summary>
        /// Insert a json serializable object to save to a collection
        /// </summary>
        /// <typeparam name="T">Any json serializable/deserializable object</typeparam>
        /// <param name="objectToInsert">Anything json serializable/deserializable object to save</param>
        /// <returns>An Int64 ID unique to the collection the object is saved in</returns>
        long Insert<T>(T objectToInsert) 
            where T : class;

        T Select<T>(long id) 
            where T : class;

        bool Update(long id, object objectToUpdate);
        bool Delete(long id);

        // Like
        IEnumerable<T> SelectLike<T>(object searchObject)
            where T : class;

        int UpdateLike(object searchObject, object newValue);

        int DeleteLike(object searchObject);

        // Between
        IEnumerable<T> SelectBetween<T>(object startRange, object endRange) 
            where T : class;

        int UpdateBetween(object startRange, object endRange, object newValue);

        int DeleteBetween(object startRange, object endRange);
    }
}
