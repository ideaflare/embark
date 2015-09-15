using System;
using System.IO;
using System.Threading;

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
