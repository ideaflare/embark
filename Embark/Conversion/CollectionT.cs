using Embark.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Conversion
{
    /// <summary>
    /// Type specific interface to CRUD and other data commands to <see cref="ITextDataStore"/> and <seealso cref="ITextConverter"/>
    /// <typeparam name="T">The POCO class of the documents</typeparam>
    /// </summary>
    public class Collection<T> where T : class
    {
        /// <summary>
        /// Create a new instance of a type specific collection
        /// </summary>
        /// <param name="collection">Basic underlying collection called with type T</param>
        public Collection(Collection collection)
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
        /// Text converter used by collection to serialize/deserialize to/from the <see cref="ITextDataStore"/>
        /// </summary>
        public ITextConverter TextConverter
        {
            get
            {
                return collection.TextConverter;
            }
        }
        
        /// <summary>
        /// Insert a new POCO object into the collection
        /// </summary>
        /// <param name="objectToInsert">The object to insert</param>
        /// <returns>The ID of the new document</returns>
        public long Insert(T objectToInsert) 
        {
            return collection.Insert(objectToInsert);
        }
        
        /// <summary>
        /// Update a entry in the collection
        /// </summary>
        /// <param name="id">The ID of the document</param>
        /// <param name="objectToUpdate">New value for the whole document. Increment/Differential updating is not supported (yet).</param>
        /// <returns>True if the document was updated</returns>
        public bool Update(long id, T objectToUpdate)
        {
            return collection.Update(id, objectToUpdate);
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
            return collection.Get<T>(id);
        }

        /// <summary>
        /// Select an existing entry in the collection, and return it in a <see cref="DocumentWrapper{T}"/>
        /// </summary>
        /// <param name="id">The Int64 ID of the document</param>
        /// <returns>The document wrapper that contains the entity</returns>
        public DocumentWrapper<T> GetWrapper(long id)
        {
            return collection.GetWrapper<T>(id);
        }
        
        /// <summary>
        /// Select all documents in the collection
        /// </summary>
        /// <returns>A collection of <see cref="DocumentWrapper{T}"/> objects. <seealso cref="ExtensionMethods.Unwrap"/></returns>
        public IEnumerable<DocumentWrapper<T>> GetAll()
        {
            return collection.GetAll<T>();
        }
        
        /// <summary>
        /// Get similar documents that have matching property values to an example object.
        /// </summary>
        /// <param name="searchObject">Example object to compare against</param>        
        /// <returns><see cref="DocumentWrapper{T}"/> objects from the collection that match the search criterea. </returns>
        public IEnumerable<DocumentWrapper<T>> GetWhere(object searchObject)
        {
            return collection.GetWhere<T>(searchObject);
        }
        
        /// <summary>
        /// Get documents where same name property values are between values of a start and end example object
        /// </summary>
        /// <param name="startRange">The first object to compare against</param>
        /// <param name="endRange">A second object to comare values agianst to check if search is between example values</param>
        /// <returns><see cref="DocumentWrapper{T}"/> objects from the collection that are within the bounds of the search criterea.</returns>
        public IEnumerable<DocumentWrapper<T>> GetBetween(object startRange, object endRange)
        {
            return collection.GetBetween<T>(startRange, endRange);
        }
    }
}
