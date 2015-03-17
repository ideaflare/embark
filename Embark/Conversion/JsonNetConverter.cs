using Embark.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Conversion
{
    class JsonNetConverter : ITextConverter
    {
        T ITextConverter.ToObject<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text);
        }

        string ITextConverter.ToText(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        IEnumerable<string> ITextConverter.GetLikeMatches(string searchObject, IEnumerable<string> compareValues)
        {
            var query = JObject.Parse(searchObject);

            foreach (var compareValue in compareValues)
            {
                var target = JObject.Parse(compareValue);
                if (IsLikeMatch(query, target))
                    yield return compareValue;
            }
        }

        private static bool IsLikeMatch(JObject query, JObject target)
        {
            return query
                .Properties()
                .All(q => JToken.DeepEquals(q.Value, target.GetValue(q.Name)));
        }

        IEnumerable<string> ITextConverter.GetBetweenMatches(string startRange, string endRange, IEnumerable<string> compareValues)
        {
            var startQuery = JObject.Parse(startRange);
            var endQuery = JObject.Parse(endRange);

            var candidates = compareValues
                .Select(stringValue => new { obj = JObject.Parse(stringValue), text = stringValue })
                .ToList();

            foreach (var startProperty in startQuery.Properties())
            {
                var endProperty = endQuery.GetValue(startProperty.Name);

                if (endProperty == null)
                    throw new InvalidOperationException(); // TODO 1 Move validation to client input

                candidates = candidates
                    .Where(c => IsBetweenMatch(startProperty, endProperty, c.obj.GetValue(startProperty.Name)))
                    .ToList();
            }

            return candidates.Select(c => c.text);
        }

        private static bool IsBetweenMatch(JToken startProperty, JToken endProperty, JToken targetProperty)
        {
            var n = new JProperty("hute", "ehtu");



            throw new NotImplementedException();
        }

    }

}
