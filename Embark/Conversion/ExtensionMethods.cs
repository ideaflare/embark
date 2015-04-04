using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Conversion
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Deserialize a serialized byte[] 
        /// </summary>
        public static byte[] GetByteArray(this object blob)
        {
            var list = ((ArrayList)blob);

            int[] deserialized = (int[])list.ToArray(typeof(int));

            return deserialized
                .Select(i => Convert.ToByte(i))
                .ToArray();
        }

        public static IEnumerable<T> Unwrap<T>(this IEnumerable<DocumentWrapper<T>> documents)
        {
            return documents.Select(doc => doc.Value);
        }
    }
}
