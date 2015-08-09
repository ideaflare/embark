using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Embark.Interaction;
using EmbarkTests._Mocks;

namespace EmbarkTests.Interaction
{
    [TestClass]
    public class CollectionTestsRange
    {
        [TestMethod]
        public void GetAll_ReturnsAllItems()
        {
            // arrange
            var allTestCollection = MockDB.SharedClient["SelectAll"];
            var testHerd = Sheep.GetTestHerd(5);

            var wrappedHerd = new List<WrappedSheep>();

            foreach (var sheep in testHerd)
            {
                var id = allTestCollection.Insert(sheep);
                wrappedHerd.Add(new WrappedSheep { ID = id, Sheep = sheep });
            }

            // act
            var querySheep = allTestCollection
                .GetAll<Sheep>()
                .OrderBy(s => s.ID)
                .ToList();

            var unwrappedHerd = querySheep
                .Unwrap()
                .ToArray();

            // assert
            Assert.AreEqual(testHerd.Count, querySheep.Count);

            foreach (var documentWrapper in querySheep)
            {
                var wrappedSheep = wrappedHerd.Where(ws => ws.ID == documentWrapper.ID).Single();

                Assert.IsTrue(documentWrapper.Content.Equals(wrappedSheep.Sheep));
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
            // arrange
            var allTestCollection = MockDB.SharedClient["SelectBetween"];
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

            // act
            var betweenSheep = allTestCollection
                .GetBetween<Sheep>(new { Age = 75 }, new { Age = 25 })
                .Single();

            // assert
            Assert.IsTrue(betweenSheep.Content.Equals(oldDusty));
        }

        [TestMethod]
        public void GetWhere_ReturnsMatchingDocument()
        {
            // arrange
            var oldWooly = new Sheep { Name = "Wooly", Age = 100, FavouriteIceCream = IceCream.Chocolate };
            var oldDusty = new Sheep { Name = "Dusty", Age = 100, FavouriteIceCream = IceCream.Chocolate };
            var youngLassy = new Sheep { Name = "Lassy", Age = 1, FavouriteIceCream = IceCream.Bubblegum };

            MockDB.BasicCollection.Insert(oldWooly);
            MockDB.BasicCollection.Insert(oldDusty);
            MockDB.BasicCollection.Insert(youngLassy);

            // act            

            IEnumerable<Sheep> matchQuery = MockDB.BasicCollection.GetWhere<Sheep>(new { Age = 100 }).Unwrap();

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
