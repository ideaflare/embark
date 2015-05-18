using System;
using System.Linq;

namespace Embark.TextConversion
{
    internal static class Validation
    {
        public static void ValidateUpload<T>(this ITextConverter converter, T sourceObject, string convertedText)
        {
            var reconvertedObject = converter.ToObject<object>(convertedText);

            var reconvertToText = converter.ToText(reconvertedObject);

            if (convertedText != reconvertToText)
                throw new ArgumentException(string.Format("Could not convert {0} consistently.\r\n"
                    + "Either the object is not a POCO with public get and set properties,"
                    + "or your ITextConverter {1} does not suppport the type {2}.",
                    sourceObject.ToString(), converter.ToString(), typeof(T).ToString()));
        }
    }
}
