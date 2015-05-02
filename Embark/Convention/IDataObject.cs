using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Embark.Interaction;

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
