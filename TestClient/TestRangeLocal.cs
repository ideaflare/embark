using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClient.IO;
using System.Collections.Generic;

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
            var tag = "sheep";
                        
            // act            
            long id = Channel.localCache.Insert(tag, oldWooly);
            long id2 = Channel.localCache.Insert(tag, oldDusty);
            long id3 = Channel.localCache.Insert(tag, youngLassy);

            IEnumerable<Sheep> matchQuery = Channel.localCache.GetWhere<Sheep>(tag, new { Age = 100 });

            var ancients = matchQuery.ToList();

            // assert
            Assert.AreEqual(2, ancients.Count);
            Assert.IsFalse(ancients.Any(s => s.Age != 100));
            Assert.IsFalse(ancients.Any(s => s.Name == "Lassy"));
            Assert.IsTrue(ancients.Any(s => s.Name == "Wooly"));
            Assert.IsTrue(ancients.Any(s => s.Name == "Dusty"));

            // cleanup
            Channel.localCache.Delete(tag, id);
            Channel.localCache.Delete(tag, id2);
            Channel.localCache.Delete(tag, id3);
        }
    }
}
