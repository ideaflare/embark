using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Conversion
{
    public class DeserializedHelper
    {
        public static byte[] GetByteArray(object blob)
        {
            var list = ((ArrayList)blob);

            int[] deserialized = (int[])list.ToArray(typeof(int));

            return deserialized
                .Select(i => Convert.ToByte(i))
                .ToArray();
        }
    }
}
