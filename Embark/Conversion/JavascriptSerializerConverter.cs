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
            return serializer.Deserialize<Dictionary<string, object>>(text);
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

        // TODO simplify this method ! Consider F# lib if it would greatly reduce code
        public bool IsBetweenMatch(object startLookup, object endLookup, object compareValue)
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
                return IsBetweenSerializedObjects(startLookup, endLookup, compareValue);
            }
            throw new NotImplementedException();
        }

        private bool IsBetweenSerializedObjects(object a, object b, object between)
        {
            if (a.GetType() == between.GetType() && between.GetType() == b.GetType())
            {
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
                    if (ea == null)
                        throw new Exception("Unknown conditon, TODO check at serialisation input @ TextFileRepo and/or Collection insert for valid comparable types.");
                    var eb = b as IEnumerable;
                    var ebetween = between as IEnumerable;

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
            else return false; // Consider throwing invalid input(Type mismatch on same property name) error, not in Converter but in Collection prior to calling local/web API. 
        }
    }
}
