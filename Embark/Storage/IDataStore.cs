using System.Collections.Generic;
using Embark.DataChannel;

namespace Embark.Storage
{
    /// <summary>
    /// Persistence for CRUD operations to a storage medium
    /// </summary>
    public interface IDataStore
    {
        /// <summary>
        /// Get all the Collections
        /// </summary>
        IEnumerable<string> Collections { get; }

        /// <summary>
        /// Insert a new object
        /// </summary>
        /// <param name="tag">The collection name to save to</param>
        /// <param name="id">ID to associate with the object</param>
        /// <param name="objectToInsert">Json or other text value to save</param>
        void Insert(string tag, long id, string objectToInsert);

        /// <summary>
        /// Update an existing object
        /// </summary>
        /// <param name="tag">The collection name the object resides in</param>
        /// <param name="id">The ID of the object to update</param>
        /// <param name="objectToUpdate">The Json or other text to replace the old value with</param>
        /// <returns>TRUE if the object was updated, FALSE if it did not exist.</returns>
        bool Update(string tag, long id, string objectToUpdate);

        /// <summary>
        /// Delete an existing object
        /// </summary>
        /// <param name="tag">The collection name the object resides in</param>
        /// <param name="id">The ID of the object to delete</param>
        /// <returns>TRUE if deleted, FALSE if it did not exist.</returns>
        bool Delete(string tag, long id);

        /// <summary>
        /// Retrieve the saved text of the object
        /// </summary>
        /// <param name="tag">Collection name the object is saved in</param>
        /// <param name="id">The ID of the object</param>
        /// <returns>NULL if the object doesn't exist, or a string associated with the object.</returns>
        string Get(string tag, long id);

        /// <summary>
        /// Return all the <see cref="DataEnvelope"/> objects that are saved in the collection
        /// </summary>
        /// <param name="tag">Name of the collection to get all objects from</param>
        /// <returns>All objects as <see cref="DataEnvelope"/> objects.</returns>
        DataEnvelope[] GetAll(string tag);
    }
}