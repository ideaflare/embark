using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Conversion
{
    /// <summary>
    /// Helper methods dealing with serialized byte[] and query IO
    /// </summary>
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

        /// <summary>
        /// Return only the deserialized objects within a wrapped document request
        /// </summary>
        /// <typeparam name="T">The type of wrapped documents</typeparam>
        /// <param name="documents">Documents with ID/Timestamp info</param>
        /// <returns>The .Value Properties of the DocumentWrappers</returns>
        public static IEnumerable<T> Unwrap<T>(this IEnumerable<DocumentWrapper<T>> documents)
        {
            return documents.Select(doc => doc.Value);
        }
    }
}
