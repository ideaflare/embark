using System;
using System.Linq;
using System.Collections.Generic;

namespace Embark.Interaction
{
    /// <summary>
    /// Type specific interface to CRUD and other data commands to <see cref="Embark.DataChannel.ITextRepository"/> and <seealso cref="Embark.TextConversion.ITextConverter"/>
    /// <typeparam name="T">The POCO class that implements <see cref="IDataEntry"/> or inherits from <see cref="DataEntryBase"/></typeparam>
    /// </summary>
    public class DataEntryCollection<T> where T : class, IDataEntry
    {
        /// <summary>
        /// Create a new instance of a type specific collection
        /// </summary>
        /// <param name="collection">Basic underlying collection called with type T</param>
        internal DataEntryCollection(Collection collection)
        {
            this.collection = collection;
        }

        private Collection collection;

        /// <summary>
        /// Get the basic collection used internally
        /// </summary>
        /// <returns><see cref="Collection"/> basic CRUD and other data methods interface</returns>
        public Collection AsBaseCollection() { return this.collection; }

        /// <summary>
        /// Insert a new POCO object into the collection
        /// </summary>
        /// <param name="objectToInsert">The object to insert</param>
        /// <returns>A referenc of the inserted document with updated ID</returns>
        public T Insert(T objectToInsert)
        {
            var id = collection.Insert(objectToInsert);
            objectToInsert.ID = id;
            return objectToInsert;
        }

        /// <summary>
        /// Update a entry in the collection
        /// </summary>
        /// <param name="objectToUpdate">New value for the whole document. Increment/Differential updating is not supported (yet).</param>
        /// <returns>True if the document was updated</returns>
        public bool Update(T objectToUpdate)
        {
            return collection.Update(objectToUpdate.ID, objectToUpdate);
        }

        /// <summary>
        /// Remove an entry from the collection and sets the object to null
        /// </summary>
        /// <param name="objectToDelete">The object to delete</param>
        /// <returns>True if the object was deleted, otherwise returns false.</returns>
        public bool Delete(T objectToDelete) 
        {
            return collection.Delete(objectToDelete.ID);
        }

        /// <summary>
        /// Remove an entry from the collection
        /// </summary>
        /// <param name="id">The ID of the document</param>
        /// <returns>True if the document was successfully removed.</returns>
        public bool Delete(long id)
        {
            return collection.Delete(id);
        }

        /// <summary>
        /// Select an existing entry in the collection
        /// </summary>
        /// <param name="id">The Int64 ID of the document</param>
        /// <returns>The object entry saved in the document</returns>
        public T Get(long id)
        {
            var item = collection.Get<T>(id);
            if(item != null)
                item.ID = id;
            return item;
        }

        /// <summary>
        /// Select an existing entry in the collection, and return it in a <see cref="DocumentWrapper{T}"/>
        /// </summary>
        /// <param name="id">The Int64 ID of the document</param>
        /// <returns>The document wrapper that contains the entity</returns>
        public DocumentWrapper<T> GetWrapper(long id)
        {
            var wrapper = collection.GetWrapper<T>(id);
            wrapper.Content.ID = id;
            return wrapper;
        }

        /// <summary>
        /// Select all documents in the collection
        /// </summary>
        /// <returns>A collection of <see cref="IDataEntry"/> objects. <seealso cref="TypeConversion.Unwrap"/></returns>
        public IEnumerable<T> GetAll()
        {
            return collection
                .GetAll<T>()
                .UnwrapWithIDs();
        }

        /// <summary>
        /// Get similar documents that have matching property values to an example object.
        /// </summary>
        /// <param name="searchObject">Example object to compare against</param>        
        /// <returns><see cref="IDataEntry"/> objects from the collection that match the search criterea. </returns>
        public IEnumerable<T> GetWhere(object searchObject)
        {
            return collection
                .GetWhere<T>(searchObject)
                .UnwrapWithIDs();
        }

        /// <summary>
        /// Get documents where same name property values are between values of a start and end example object
        /// </summary>
        /// <param name="startRange">The first object to compare against</param>
        /// <param name="endRange">A second object to comare values agianst to check if search is between example values</param>
        /// <returns><see cref="IDataEntry"/> objects from the collection that are within the bounds of the search criterea.</returns>
        public IEnumerable<T> GetBetween(object startRange, object endRange)
        {
            return collection
                .GetBetween<T>(startRange, endRange)
                .UnwrapWithIDs();
        }
    }
}
