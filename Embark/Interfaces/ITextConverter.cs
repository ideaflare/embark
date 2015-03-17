using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Embark.Interfaces
{
    public interface ITextConverter
    {
        T ToObject<T>(string text);

        string ToText(object obj);

        IEnumerable<string> GetLikeMatches(string searchObject, IEnumerable<string> compareValues);

        IEnumerable<string> GetBetweenMatches(string startRange, string endRange, IEnumerable<string> compareValues);
    }
}
