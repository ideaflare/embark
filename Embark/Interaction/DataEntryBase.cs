using System;
using Embark.Interaction.MVVM;

namespace Embark.Interaction
{
    /// <summary>
    /// Basic implementation of <see cref="IDataEntry"/> used for <see cref="DataEntryCollection{T}"/>
    /// </summary>
    public abstract class DataEntryBase : PropertyChangeBase, IDataEntry
    {
        /// <summary>
        /// Unique Document identifier used by Embark
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// The creation time of the document
        /// <remarks>
        /// <para>
        /// Timestamp depends on the resolution time of the machine the document is created on.
        /// </para>
        /// <para>
        /// Usually accurate to within 15 milliseconds of actual created date.
        /// </para>
        /// </remarks>
        /// </summary>
        public DateTime Timestamp => new DateTime(ID);

        /// <summary>
        /// Call Collection.Update(this) whenever this objects PropertyChanged event gets fired.
        /// <para>Not recommended use for UI related data, users want to confirm their changes.</para>
        /// <para>NOTE: Calling this method multiple times and/or collections subscribes multiple collection.Update(this) method calls.</para>
        /// </summary>
        /// <typeparam name="T">Inherited class of DataEntryBase</typeparam>
        /// <param name="dataEntryCollection">DataEntry colletion to save changes to.</param>
        public void RegisterAutoUpdate<T>(DataEntryCollection<T> dataEntryCollection) where T : class, IDataEntry
            => PropertyChanged += (s, e) => dataEntryCollection.AsBaseCollection().Update(ID, this);
    }
}
