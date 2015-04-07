using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Storage
{
    /// <summary>
    /// Used to send public get and set properties of ID and Text.
    /// </summary>
    /// <remarks> Used instead of Tuple or KeyValuePair,
    /// because their key/item properties are get only,
    /// and any unknown object/text serializer can be passed to Embark that might not serialize/deserialize non-POCO classes correctly</remarks>
    public class DataEnvelope
    {
        /// <summary>
        /// An Int64 ID Unique to the document
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// JSON (XML, YAML or other) Text repesenting the serialized POCO
        /// </summary>
        public string Text { get; set; }
    }
}
