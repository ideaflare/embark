using System;
using System.Collections.Generic;

namespace Embark.Interfaces
{
    /// <summary>
    /// All commands to insert/get/update/delete documents
    /// </summary>
    public interface IChannel
    {
        // Basic

        /// <summary>
        /// Insert a json serializable object to save to a collection
        /// </summary>
        /// <typeparam name="T">Any json serializable/deserializable object</typeparam>
        /// <param name="tag">A name of the collection to save it in</param>
        /// <param name="objectToInsert">Anything json serializable/deserializable object to save</param>
        /// <returns>An Int64 ID unique to the collection the object is saved in</returns>
        long Insert<T>(string tag, T objectToInsert) where T : class;
        T Get<T>(string tag, long id) where T : class;

        bool Update(string tag, long id, object objectToUpdate);
        bool Delete(string tag, long id);

        // Range
        IEnumerable<T> GetWhere<T>(string tag, object searchObject, object optionalEndrange = null) where T : class;

        int UpdateWhere(string tag, object newValue, object searchObject, object optionalEndrange = null);
        int DeleteWhere(string tag, object searchObject, object optionalEndrange = null);
    }
}
