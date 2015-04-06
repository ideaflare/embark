using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Storage
{
    /// <summary>
    /// Used to send public get&set properties of ID & Text.
    /// </summary>
    /// <remarks> Used instead of Tuple or KeyValuePair,
    /// because their key/item properties are get only,
    /// and any unknown object/text serializer can be passed to Embark that might not serialize/deserialize non-POCO classes correctly</remarks>
    public class DataEnvelope
    {
        public long ID { get; set; }
        public string Text { get; set; }
    }
}
