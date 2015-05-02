namespace Embark.Convention
{
    /// <summary>
    /// Used by <see cref="DocumentCollection{T}"/> to ensure class has expected Int64 ID
    /// </summary>
    public interface IDataObject
    {
        /// <summary>
        /// Unique ID used by EmbarkDB 
        /// </summary>
        long ID { get; set; }
    }
}
