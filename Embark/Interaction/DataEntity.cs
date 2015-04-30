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
    /// Basic implementation of <see cref="IDataEntity"/>
    /// <para>
    /// Also has related ID/Entity methods and 
    /// </para>
    /// </summary>
    public abstract class DataEntity : NotifyChangeBase, IDataEntity
    {
        /// <summary>
        /// Unique Document identifier used by Embark
        /// </summary>
        public long ID { get; set; }
    }
}
