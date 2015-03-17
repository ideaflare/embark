using Embark.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Embark.Conversion
{
    class JavascriptSerializerConverter : ITextConverter
    {
        public JavascriptSerializerConverter()
        {
            this.serializer = new JavaScriptSerializer();
        }

        private JavaScriptSerializer serializer;

        T ITextConverter.ToObject<T>(string text)
        {
            return serializer.Deserialize<T>(text);
        }

        string ITextConverter.ToText(object obj)
        {
            var text = serializer.Serialize(obj);
            return TextFormatter.JsonPrettyPrint(text);
        }

        IEnumerable<string> ITextConverter.GetLikeMatches(string searchObject, IEnumerable<string> compareValues)
        {
            var propertyLookup = serializer.Deserialize<Dictionary<string, object>>(searchObject);

            return compareValues
                .Select(txt => new
                {
                    text = txt,
                    graph = serializer.Deserialize<Dictionary<string, object>>(txt)
                })
                .Where(comparison => IsMatch(propertyLookup, comparison.graph))
                .Select(c => TextFormatter.JsonPrettyPrint(c.text));
        }

        public bool IsMatch(object a, object b)
        {
            var dA = a as Dictionary<string, object>;

            if (dA != null)
            {
                var dB = b as Dictionary<string, object>;

                if (dB != null)
                {
                    foreach (var keyValue in dA)
                    {
                        object bValue;
                        if (dB.TryGetValue(keyValue.Key, out bValue))
                        {
                            if (!IsMatch(keyValue.Value, bValue))
                                return false;
                        }
                        else return false;
                    }
                    return true;
                }
                else return false;
            }
            else return a.Equals(b);
        }
        
        IEnumerable<string> ITextConverter.GetBetweenMatches(string startRange, string endRange, IEnumerable<string> compareValues)
        {
            throw new NotImplementedException();
        }
    }
}
