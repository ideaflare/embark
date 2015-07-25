using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TestClient.TestData;
using Embark.Interaction;
using TestClient.TestData.Basic;

namespace TestClient
{
    [TestClass]
    public class CollectionRange
    {
        [TestMethod]
        public void GetAll_ReturnsAllItems()
        {
            //Arrange
            var allTestCollection = Cache.localClient["SelectAll"];
            var testHerd = TestEntities.GetTestHerd(5);

            var wrappedHerd = new List<WrappedSheep>();

            foreach (var sheep in testHerd)
            {
                var id = allTestCollection.Insert(sheep);
                wrappedHerd.Add(new WrappedSheep { ID = id, Sheep = sheep });
            }

            //Act
            var querySheep = allTestCollection.GetAll<Sheep>();

            var unwrappedHerd = querySheep.Unwrap().ToArray();

            //Assert
            Assert.AreEqual(testHerd.Count, querySheep.Count());

            foreach(var documentWrapper in querySheep)
            {
                var wrappedSheep = wrappedHerd.Where(ws => ws.ID == documentWrapper.ID).Single();

                Assert.IsTrue(documentWrapper.Content.Equals( wrappedSheep.Sheep));
            }

            // Assumption that insert order = fetch order. If this changes, change the unit test and allow unordered insert & query.
            for (int i = 0; i < testHerd.Count; i++)
            {
                Assert.IsTrue(testHerd[i].Equals(unwrappedHerd[i]));
            }
        }

        [TestMethod]
        public void GetBetween_ReturnsBetweenItems()
        {
            //Arrange
            var allTestCollection = Cache.localClient["SelectBetween"];
            var testHerd = new List<Sheep>();

            var oldWooly = new Sheep { Name = "Wooly", Age = 100, FavouriteIceCream = IceCream.Chocolate };
            var oldDusty = new Sheep { Name = "Dusty", Age = 50, FavouriteIceCream = IceCream.Chocolate };
            var youngLassy = new Sheep { Name = "Lassy", Age = 1, FavouriteIceCream = IceCream.Bubblegum };

            testHerd.Add(oldWooly);
            testHerd.Add(oldDusty);
            testHerd.Add(youngLassy);

            var wrappedHerd = new List<WrappedSheep>();
            foreach (var sheep in testHerd)
            {
                var id = allTestCollection.Insert(sheep);
                wrappedHerd.Add(new WrappedSheep { ID = id, Sheep = sheep });
            }

            //Act
            var betweenSheep = allTestCollection
                .GetBetween<Sheep>(new { Age = 75 }, new { Age = 25 })
                .Single();

            //Assert
            Assert.IsTrue(betweenSheep.Content.Equals(oldDusty));
        }

        [TestMethod]
        public void GetWhere_ReturnsMatchingDocument()
        {
            // arrange
            var oldWooly = new Sheep { Name = "Wooly", Age = 100, FavouriteIceCream = IceCream.Chocolate };
            var oldDusty = new Sheep { Name = "Dusty", Age = 100, FavouriteIceCream = IceCream.Chocolate };
            var youngLassy = new Sheep { Name = "Lassy", Age = 1, FavouriteIceCream = IceCream.Bubblegum };
                        
            Cache.BasicCollection.Insert(oldWooly);
            Cache.BasicCollection.Insert(oldDusty);
            Cache.BasicCollection.Insert(youngLassy);

            // act            

            IEnumerable<Sheep> matchQuery = Cache.BasicCollection.GetWhere<Sheep>(new { Age = 100 }).Unwrap();

            var ancients = matchQuery.ToList();

            // assert
            Assert.AreEqual(2, ancients.Count);

            Assert.IsFalse(ancients.Any(s => s.Age != 100));
            Assert.IsFalse(ancients.Any(s => s.Name == "Lassy"));

            Assert.IsTrue(ancients.Any(s => s.Name == "Wooly"));
            Assert.IsTrue(ancients.Any(s => s.Name == "Dusty"));
        }        
    }
}
