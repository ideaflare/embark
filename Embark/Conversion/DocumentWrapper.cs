using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Convension
{
    /// <summary>
    /// Type wrapper with timestamp & ID that might be required for subsequent queries.
    /// </summary>
    public class DocumentWrapper<T>
    {
        public long ID { get; internal set; }

        private object obj;
        public object Value
        {
            get { return (T)obj; }
            internal set { obj = value; }
        }

        public DateTime Timestamp { get { return new DateTime(ID); } }
    }

}
