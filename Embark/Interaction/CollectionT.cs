using System.Collections.Generic;

namespace Embark.Interaction
{
    /// <summary>
    /// Type specific interface to CRUD and other data commands to <see cref="DataChannel.ITextRepository"/> and <seealso cref="TextConversion.ITextConverter"/>
    /// <typeparam name="T">The POCO class of the documents</typeparam>
    /// </summary>
    public class Collection<T> where T : class
    {
        /// <summary>
        /// Create a new instance of a type specific collection
        /// </summary>
        /// <param name="collection">Basic underlying collection called with type T</param>
        internal Collection(Collection collection)
        {
            this.collection = collection;
        }

        private Collection collection;

        /// <summary>
        /// Get the basic collection used internally
        /// </summary>
        /// <returns><see cref="Collection"/> basic CRUD and other data methods interface</returns>
        public Collection AsBaseCollection() => collection;
          
        /// <summary>
        /// Insert a new POCO object into the collection
        /// </summary>
        /// <param name="objectToInsert">The object to insert</param>
        /// <returns>The ID of the new document</returns>
        public long Insert(T objectToInsert) => collection.Insert(objectToInsert);
        
        /// <summary>
        /// Update a entry in the collection
        /// </summary>
        /// <param name="id">The ID of the document</param>
        /// <param name="objectToUpdate">New value for the whole document. Increment/Differential updating is not supported (yet).</param>
        /// <returns>True if the document was updated</returns>
        public bool Update(long id, T objectToUpdate) => collection.Update(id, objectToUpdate);

        /// <summary>
        /// Remove an entry from the collection
        /// </summary>
        /// <param name="id">The ID of the document</param>
        /// <returns>True if the document was successfully removed.</returns>
        public bool Delete(long id) => collection.Delete(id);

        /// <summary>
        /// Select an existing entry in the collection
        /// </summary>
        /// <param name="id">The Int64 ID of the document</param>
        /// <returns>The object entry saved in the document</returns>
        public T Get(long id) => collection.Get<T>(id);

        /// <summary>
        /// Select an existing entry in the collection, and return it in a <see cref="DocumentWrapper{T}"/>
        /// </summary>
        /// <param name="id">The Int64 ID of the document</param>
        /// <returns>The document wrapper that contains the entity if it exists, otherwise returns NULL</returns>
        public DocumentWrapper<T> GetWrapper(long id) => collection.GetWrapper<T>(id);

        /// <summary>
        /// Select all documents in the collection
        /// </summary>
        /// <returns>A collection of <see cref="DocumentWrapper{T}"/> objects. <seealso cref="DocumentWrapperExtensions.Unwrap"/></returns>
        public IEnumerable<DocumentWrapper<T>> GetAll() => collection.GetAll<T>();

        /// <summary>
        /// Get similar documents that have matching property values to an example object.
        /// </summary>
        /// <param name="searchObject">Example object to compare against</param>        
        /// <returns><see cref="DocumentWrapper{T}"/> objects from the collection that match the search criterea. </returns>
        public IEnumerable<DocumentWrapper<T>> GetWhere(object searchObject) => collection.GetWhere<T>(searchObject);

        /// <summary>
        /// Get documents where same name property values are between values of a start and end example object
        /// </summary>
        /// <param name="startRange">The first object to compare against</param>
        /// <param name="endRange">A second object to comare values agianst to check if search is between example values</param>
        /// <returns><see cref="DocumentWrapper{T}"/> objects from the collection that are within the bounds of the search criterea.</returns>
        public IEnumerable<DocumentWrapper<T>> GetBetween(object startRange, object endRange) => collection.GetBetween<T>(startRange, endRange);
    }
}
