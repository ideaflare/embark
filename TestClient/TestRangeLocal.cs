using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClient.IO;
using System.Collections.Generic;
using TestClient.IO.TestData;
using TestClient.TestData;

namespace TestClient
{
    [TestClass]
    public class TestRangeLocal
    {
        [TestMethod]
        public void GetWhereMatch()
        {
            // arrange
            var oldWooly = new Sheep { Name = "Wooly", Age = 100, FavouriteIceCream = IceCream.Chocolate };
            var oldDusty = new Sheep { Name = "Dusty", Age = 100, FavouriteIceCream = IceCream.Chocolate };
            var youngLassy = new Sheep { Name = "Lassy", Age = 1, FavouriteIceCream = IceCream.Bubblegum };
                        
            // act            
            long id = Cache.localSheep.Insert(oldWooly);
            long id2 = Cache.localSheep.Insert(oldDusty);
            long id3 = Cache.localSheep.Insert(youngLassy);

            IEnumerable<Sheep> matchQuery = Cache.localSheep.SelectLike<Sheep>(new { Age = 100 });

            var ancients = matchQuery.ToList();

            // assert
            Assert.AreEqual(2, ancients.Count);

            Assert.IsFalse(ancients.Any(s => s.Age != 100));
            Assert.IsFalse(ancients.Any(s => s.Name == "Lassy"));

            Assert.IsTrue(ancients.Any(s => s.Name == "Wooly"));
            Assert.IsTrue(ancients.Any(s => s.Name == "Dusty"));

            // cleanup
            Cache.localSheep.Delete(id);
            Cache.localSheep.Delete(id2);
            Cache.localSheep.Delete(id3);
        }
    }
}
