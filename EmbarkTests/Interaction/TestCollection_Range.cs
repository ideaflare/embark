using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Embark.Interaction;
using EmbarkTests._Mocks;

namespace EmbarkTests.Interaction
{
    public class TestCollection_Range
    {
        [Fact]
        public void GetAll_ReturnsAllItems()
        {
            // arrange
            var allTestCollection = MockDB.SharedRuntimeClient["SelectAll"];
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
            Assert.Equal(testHerd.Count, querySheep.Count);

            foreach (var documentWrapper in querySheep)
            {
                var wrappedSheep = wrappedHerd.Where(ws => ws.ID == documentWrapper.ID).Single();

                Assert.True(documentWrapper.Content.Equals(wrappedSheep.Sheep));
            }

            // Assumption that insert order = fetch order. If this changes, change the unit test and allow unordered insert & query.
            for (int i = 0; i < testHerd.Count; i++)
            {
                Assert.True(testHerd[i].Equals(unwrappedHerd[i]));
            }
        }

        [Fact]
        public void GetBetween_ReturnsBetweenItems()
        {
            // arrange
            var allTestCollection = MockDB.SharedRuntimeClient["SelectBetween"];
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
            Assert.True(betweenSheep.Content.Equals(oldDusty));
        }

        [Fact]
        public void GetWhere_ReturnsMatchingDocument()
        {
            // arrange
            var oldWooly = new Sheep { Name = "Wooly", Age = 100, FavouriteIceCream = IceCream.Chocolate };
            var oldDusty = new Sheep { Name = "Dusty", Age = 100, FavouriteIceCream = IceCream.Chocolate };
            var youngLassy = new Sheep { Name = "Lassy", Age = 1, FavouriteIceCream = IceCream.Bubblegum };

            MockDB.RuntimeBasicCollection.Insert(oldWooly);
            MockDB.RuntimeBasicCollection.Insert(oldDusty);
            MockDB.RuntimeBasicCollection.Insert(youngLassy);

            // act            

            IEnumerable<Sheep> matchQuery = MockDB.RuntimeBasicCollection.GetWhere<Sheep>(new { Age = 100 }).Unwrap();

            var ancients = matchQuery.ToList();

            // assert
            Assert.Equal(2, ancients.Count);

            Assert.False(ancients.Any(s => s.Age != 100));
            Assert.False(ancients.Any(s => s.Name == "Lassy"));

            Assert.True(ancients.Any(s => s.Name == "Wooly"));
            Assert.True(ancients.Any(s => s.Name == "Dusty"));
        }
    }
}
