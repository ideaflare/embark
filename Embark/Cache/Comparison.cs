using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Cache
{
    internal class Comparison
    {
        public static bool IsMatch(JObject query, JObject target)
        {
            return query
                .Properties()
                .All(q => JToken.DeepEquals(q.Value, target.GetValue(q.Name)));
        }
    }
}
