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
    public interface ITextDataStore
    {
        // Basic

        /// <summary>
        /// Insert a json serializable object to save to a collection
        /// </summary>
        /// <typeparam name="T">Any json serializable/deserializable object</typeparam>
        /// <param name="tag">A name of the collection to save it in</param>
        /// <param name="objectToInsert">Anything json serializable/deserializable object to save</param>
        /// <returns>An Int64 ID unique to the collection the object is saved in</returns>
        long Insert(string tag, string objectToInsert);

        string Select(string tag, long id);

        bool Update(string tag, long id, string objectToUpdate);
        bool Delete(string tag, long id);

        // Like
        IEnumerable<string> SelectLike(string tag, string searchObject);

        int UpdateLike(string tag, string searchObject, string newValue);

        int DeleteLike(string tag, string searchObject);

        // Between
        IEnumerable<string> SelectBetween(string tag, string startRange, string endRange);

        int UpdateBetween(string tag, string startRange, string endRange, string newValue);

        int DeleteBetween(string tag, string startRange, string endRange);
    }
}
