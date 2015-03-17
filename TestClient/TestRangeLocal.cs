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
        public void GetSelectLike()
        {
            // arrange
            var oldWooly = new Sheep { Name = "Wooly", Age = 100, FavouriteIceCream = IceCream.Chocolate };
            var oldDusty = new Sheep { Name = "Dusty", Age = 100, FavouriteIceCream = IceCream.Chocolate };
            var youngLassy = new Sheep { Name = "Lassy", Age = 1, FavouriteIceCream = IceCream.Bubblegum };
                        
            long id = Cache.localSheep.Insert(oldWooly);
            long id2 = Cache.localSheep.Insert(oldDusty);
            long id3 = Cache.localSheep.Insert(youngLassy);

            // act            

            IEnumerable<Sheep> matchQuery = Cache.localSheep.SelectLike<Sheep>(new { Age = 100 });

            var ancients = matchQuery.ToList();

            // assert
            Assert.AreEqual(2, ancients.Count);

            Assert.IsFalse(ancients.Any(s => s.Age != 100));
            Assert.IsFalse(ancients.Any(s => s.Name == "Lassy"));

            Assert.IsTrue(ancients.Any(s => s.Name == "Wooly"));
            Assert.IsTrue(ancients.Any(s => s.Name == "Dusty"));
        }

        //[TestMethod]
        //public void DeleteSelectLike()
        //{
        //    // arrange
        //    var shakes = new Sheep { Name = "Shakes", Age = 50 };
        //    var shocks = new Sheep { Name = "Shocks", Age = 50 };
        //    var shiny = new Sheep { Name = "Shiny", Age = 40};

        //    long id = Cache.localSheep.Insert(shakes);
        //    long id2 = Cache.localSheep.Insert(shocks);
        //    long id3 = Cache.localSheep.Insert(shiny);

        //    // act            

        //    Cache.localSheep.DeleteLike(new { Age = 50 });

        //    IEnumerable<Sheep> matchQuery = Cache.localSheep.SelectLike<Sheep>(new { Age = 50 });

        //    var halfCenturySheep = matchQuery.ToList();

        //    // assert
        //    Assert.AreEqual(0, halfCenturySheep.Count);

        //    Assert.IsTrue(halfCenturySheep.Any(s => s.Age != 50));
        //    Assert.IsTrue(halfCenturySheep.Any(s => s.Name == "Shiny"));

        //    Assert.IsFalse(halfCenturySheep.Any(s => s.Name == "Shakes"));
        //    Assert.IsFalse(halfCenturySheep.Any(s => s.Name == "Shocks"));
        //}
    }
}
