using Embark.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Conversion
{
    /// <summary>
    /// Type wrapper with timestamp & ID that might be required for subsequent queries.
    /// </summary>
    public class DocumentWrapper<T>
    {
        internal DocumentWrapper(DataEnvelope dataEnvelope, Collection collection)
        {
            this.ID = dataEnvelope.ID;
            this.Value = collection.textConverter.ToObject<T>(dataEnvelope.Text);
            this.collection = collection;
        }

        public long ID { get; internal set; }

        public T Value { get; set; }

        public DateTime Timestamp { get { return new DateTime(ID); } }

        //public void Update() { }
        private Embark.Conversion.Collection collection;

        // ?
        //public T Unwrap() { return Value; }
    }

}
