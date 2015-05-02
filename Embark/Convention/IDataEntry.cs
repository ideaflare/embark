namespace Embark.Convention
{
    /// <summary>
    /// Used by <see cref="DataEntryCollection{T}"/> to ensure class has expected Int64 ID
    /// </summary>
    public interface IDataEntry
    {
        /// <summary>
        /// Unique ID used by EmbarkDB 
        /// </summary>
        long ID { get; set; }
    }
}
