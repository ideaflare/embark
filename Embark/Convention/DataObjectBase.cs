using System;
using Embark.Convention.MVVM;

namespace Embark.Convention
{
    /// <summary>
    /// Basic implementation of <see cref="IDataObject"/> used for <see cref="DocumentCollection{T}"/>
    /// </summary>
    public abstract class DataObjectBase : PropertyChangeBase, IDataObject
    {
        /// <summary>
        /// Unique Document identifier used by Embark
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// The creation time of the document
        /// <remarks>
        /// Timestamp depends on the resolution time of the machine the document is created on.
        /// Usually accurate to within 15 milliseconds of actual created date.
        /// </remarks>
        /// </summary>
        public DateTime Timestamp { get { return new DateTime(ID); } }
    }
}
