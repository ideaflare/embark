using System;
using Embark.DataChannel;

namespace Embark.Interaction
{
    /// <summary>
    /// Query result wrapper with Content object, meta properties, and update and delete methods.
    /// </summary>
    public class DocumentWrapper<T>
    {
        internal DocumentWrapper(DataEnvelope dataEnvelope, Collection collection)
            : this(dataEnvelope.ID, dataEnvelope.Text, collection)
        {
        }

        internal DocumentWrapper(long id, string text, Collection collection)
        {
            this.ID = id;            
            this.Content = collection.TextConverter.ToObject<T>(text);
            this.collection = collection;
        }

        //TODO 2 Create other Wrapped document requests/commands.
        private Collection collection;

        /// <summary>
        /// Int64 ID Unique to the document
        /// </summary>
        public long ID { get; internal set; }

        /// <summary>
        /// Deserialized Object contained in the wrapper
        /// </summary>
        public T Content { get; set; }

        /// <summary>
        /// Deserialized Object contained in the wrapper
        /// <para>Synonym for <see cref="Content"/></para>
        /// </summary>
        //[Obsolete("Use DocumentWrapper.Content instead")]
        public T Value
        {
            get { return this.Content; }
            set { this.Content = value; }
        }

        /// <summary>
        /// The creation time of the document
        /// <remarks>
        /// Timestamp depends on the resolution time of the machine the document is created on.
        /// Usually accurate to within 15 milliseconds of actual created date.
        /// </remarks>
        /// </summary>
        public DateTime Timestamp { get { return new DateTime(ID); } }

        /// <summary>
        /// ToString() of the object within the wrapper
        /// </summary>
        /// <returns>DocumentWrapper.Value.ToString()</returns>
        public override string ToString()
        {
            return this.Content.ToString();
        }

        /// <summary>
        /// Commit document value to the database
        /// </summary>
        public bool Update()
        {
            return this.collection.Update(this.ID, this.Content);
        }

        /// <summary>
        /// Delete the document from the database
        /// </summary>
        public void Delete()
        {
            this.collection.Delete(this.ID);
            this.Content = default(T);
        }
    }

}
