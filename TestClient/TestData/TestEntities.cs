using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestClient.TestData.Basic;
using TestClient.TestData.DataEntry;

namespace TestClient.TestData
{
    internal class TestEntities
    {
        internal static Sound GetTestSound()
        {
            return new Sound
            {
                Description = GetRandomString(),
                Quality = rnd.Value.Next(100, 1000)
            };                
        }

        internal static Sheep GetTestSheep()
        {
            var r = rnd.Value;
            return new Sheep
            {
                Name = GetRandomString(),
                Age = r.Next(1, 15),
                FavouriteIceCream = (IceCream)r.Next(0, 4)
            };
        }

        internal static List<Sheep> GetTestHerd(int count = 10)
        {
            return Enumerable.Range(0, count)
                .Select(i => GetTestSheep())
                .ToList();
        }

        internal static string GetRandomString()
        {
            return Path.GetRandomFileName().Replace(".", "");
        }

        static ThreadLocal<Random> rnd = new ThreadLocal<Random>(() => new Random());
    }
}
