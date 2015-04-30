using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Embark.Interaction;

namespace Embark.Interfaces
{
    /// <summary>
    /// Used by <see cref="EntityCollection{T}"/> to ensure class has expected Int64 ID
    /// </summary>
    public interface IDataEntity
    {
        /// <summary>
        /// Unique ID used by EmbarkDB 
        /// </summary>
        long ID { get; set; }
    }
}
