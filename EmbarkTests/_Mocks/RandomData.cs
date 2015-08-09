using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmbarkTests._Mocks
{
    class RandomData
    {
        internal static string GetRandomString()
        {
            return Path.GetRandomFileName().Replace(".", "");
        }

        internal static Random Random => rnd.Value;

        static ThreadLocal<Random> rnd = new ThreadLocal<Random>(() => new Random());
    }
}
