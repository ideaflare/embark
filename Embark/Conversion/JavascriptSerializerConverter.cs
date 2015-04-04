using Embark.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Embark.Conversion
{
    internal class JavascriptSerializerConverter : ITextConverter
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

        private bool IsMatch(object a, object b)
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
            var startLookup = serializer.Deserialize<Dictionary<string, object>>(startRange);
            var endLookup = serializer.Deserialize<Dictionary<string, object>>(endRange);

            return compareValues
               .Select(txt => new
               {
                   text = txt,
                   graph = serializer.Deserialize<Dictionary<string, object>>(txt)
               })
               .Where(comparison => IsBetweenMatch(startLookup, endLookup, comparison.graph))
               .Select(c => TextFormatter.JsonPrettyPrint(c.text));

            throw new NotImplementedException();
        }

        private bool IsBetweenMatch(object startLookup, object endLookup, object compareValue)
        {
            var sL = startLookup as Dictionary<string, object>;
            if (sL != null)
            {
                var eL = endLookup as Dictionary<string, object>;
                if (eL != null)
                {
                    var cL = compareValue as Dictionary<string, object>;
                    if (cL != null)
                    {
                        foreach (var sValue in sL)
                        {
                            object eValue;
                            if (eL.TryGetValue(sValue.Key, out eValue))
                            {
                                object cValue;
                                if (cL.TryGetValue(sValue.Key, out cValue))
                                {
                                    if (!IsBetweenMatch(sValue.Value, eValue, cValue))
                                        return false;
                                }
                            }
                            else return false;
                        }
                    }
                    return true;
                }
                else return false;
            }
            else
            {
                // compare types, cast to numeric or string
            }
            throw new NotImplementedException();
        }
    }
}
