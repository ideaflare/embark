using Embark.Interfaces;
using Embark.Storage;
using System;
using System.Collections;
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

        string ITextConverter.ToText(object obj)
        {
            var text = serializer.Serialize(obj);
            return TextFormatter.JsonPrettyPrint(text);
        }

        T ITextConverter.ToObject<T>(string text)
        {
            return serializer.Deserialize<T>(text);
        }

        object ITextConverter.ToComparisonObject(string text)
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
