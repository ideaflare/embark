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
        public DateTime Timestamp { get { return new DateTime(ID); } }
    }
}
