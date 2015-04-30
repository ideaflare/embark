using Embark.DesignPatterns.MVVM;
using Embark.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Interaction
{
    /// <summary>
    /// Basic implementation of <see cref="IDataEntity"/> used for <see cref="EntityCollection{T}"/>
    /// </summary>
    abstract class DataEntity : NotifyChangeBase, IDataEntity
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
