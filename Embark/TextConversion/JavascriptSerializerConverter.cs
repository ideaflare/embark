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
            serializer = new JavaScriptSerializer()
            {
                MaxJsonLength = int.MaxValue
            };
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
            => serializer.DeserializeObject(text);

        public bool IsMatch(object a, object b)
        {
            var lookupA = a as Dictionary<string, object>;
            var lookupB = b as Dictionary<string, object>;

            if (lookupA == null || lookupB == null)
            {
                return a.Equals(b);
            }
            else return lookupA.All((kvpA) => HasEqualProperty(lookupB, kvpA.Key, kvpA.Value));
        }

        private bool HasEqualProperty(Dictionary<string, object> lookup, string propertyName, object objectValue, object lookupValue = null)
            => lookup.TryGetValue(propertyName, out lookupValue) 
            && IsMatch(objectValue, lookupValue);

        // TODO simplify 
        public bool IsBetweenMatch(object startRange, object endRange, object compareValue)
        {
            var start = startRange as Dictionary<string, object>;
            var end = endRange as Dictionary<string, object>;
            var comparison = compareValue as Dictionary<string, object>;

            if (start == null || end == null || comparison == null)
            {
                // compare types, cast to numeric or string
                return IsBetweenSerializedObjects(startRange, endRange, compareValue);
            }
            else
            {
                return start.All(startLookup => PropertiesMatch(startLookup, end, comparison));
            }
        }

        private bool PropertiesMatch(KeyValuePair<string, object> startLookup,
            Dictionary<string, object> end, Dictionary<string, object> comparison,
            object endVal = null, object compareVal = null)
            => 
            end.TryGetValue(startLookup.Key, out endVal)
            && comparison.TryGetValue(startLookup.Key, out compareVal)
            && IsBetweenMatch(startLookup.Value, endVal, compareVal);

        // TODO simplify 
        private bool IsBetweenSerializedObjects(object a, object b, object between)
        {
            // Consider throwing invalid input(Type mismatch on same property name) error, not in Converter but in Collection prior to calling local/web API. 
            if (a.GetType() != between.GetType() || between.GetType() != b.GetType())
                return false;

            var cBetween = (IComparable)between;
            if (cBetween != null)
            {
                return IsComparableEqualOrWithinBounds(cBetween, a, b);
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

        private static bool IsComparableEqualOrWithinBounds(IComparable cBetween, object a, object b)
            => cBetween.CompareTo(a) != cBetween.CompareTo(b)
                                || cBetween.CompareTo(a) == 0
                                || cBetween.CompareTo(b) == 0;
    }
}
