using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Embark.TextConversion
{
    internal class JavascriptSerializerTextConverter : ITextConverter
    {
        public JavascriptSerializerTextConverter()
        {
            serializer = new JavaScriptSerializer();
        }

        private JavaScriptSerializer serializer;

        public string ToText(object obj)
        {
            var text = serializer.Serialize(obj);
            return JsonTextFormatter.JsonPrettyPrint(text);
        }

        public T ToObject<T>(string text)
        {
            if (typeof(T) == typeof(string) && 
                (text.StartsWith("{") || text.StartsWith("[")))
                return (T)(object)text;

            return serializer.Deserialize<T>(text);
        }

        public object ToComparisonObject(string text)
        {
            return serializer.DeserializeObject(text);
        }

        public bool IsMatch(object a, object b)
        {
            var dA = a as Dictionary<string, object>;
            var dB = b as Dictionary<string, object>;

            if (dA == null | dB == null)
            {
                return a.Equals(b);
            }
            else return dA.All((ka) => HasEqualProperty(dB, ka.Key, ka.Value));
        }

        private bool HasEqualProperty(Dictionary<string,object> lookup, string propertyName, object objectValue)
        {
            object BValue;
            if (lookup.TryGetValue(propertyName, out BValue))
            {
                return IsMatch(objectValue, BValue);
            }
            else return false;
        }

        // TODO simplify 
        public bool IsBetweenMatch(object startLookup, object endLookup, object compareValue)
        {
            var sL = startLookup as Dictionary<string, object>;
            var eL = endLookup as Dictionary<string, object>;
            var cL = compareValue as Dictionary<string, object>;

            if (sL != null && eL != null && cL != null)
            {
                foreach (var sLookup in sL)
                {
                    object eValue;
                    object cValue;

                    if (eL.TryGetValue(sLookup.Key, out eValue) &&
                        cL.TryGetValue(sLookup.Key, out cValue))
                    {
                        if (!IsBetweenMatch(sLookup.Value, eValue, cValue))
                            return false;
                    }
                    else return false;
                }
                return true;
            }
            else
            {
                // compare types, cast to numeric or string
                return IsBetweenSerializedObjects(startLookup, endLookup, compareValue);
            }
            throw new NotImplementedException();
        }

        // TODO simplify 
        private bool IsBetweenSerializedObjects(object a, object b, object between)
        {
            // Consider throwing invalid input(Type mismatch on same property name) error, not in Converter but in Collection prior to calling local/web API. 
            if (a.GetType() != between.GetType() || between.GetType() != b.GetType())
                return false;

            var ca = a as IComparable;
            if (ca != null)
            {
                var cb = (IComparable)b;
                var cBetween = (IComparable)between;

                if (ca.CompareTo(between) == 0 || cb.CompareTo(between) == 0)
                    return true;
                else return ca.CompareTo(between) != cb.CompareTo(between);
            }
            else // Compare if array is between two other arrays ? Edge case, rethink what it means and what is an expected IsBetween comparison.
            {
                var ea = a as IEnumerable;
                var eb = b as IEnumerable;
                var ebetween = between as IEnumerable;

                if (ebetween == null)
                    throw new Exception("Unknown conditon, TODO check at serialisation input @ TextFileRepo and/or Collection insert for valid comparable types.");

                var la = ea.Cast<object>().ToArray();
                var lb = eb.Cast<object>().ToArray();
                var lbetween = ebetween.Cast<object>().ToArray();

                var minLength = (int)Math.Min(Math.Min(la.Length, lb.Length), lbetween.Length);
                for (int i = 0; i < minLength; i++)
                {
                    if (!IsBetweenSerializedObjects(la[i], lb[i], lbetween[i]))
                        return false;
                }
                return true;
            }
        }

    }
}
