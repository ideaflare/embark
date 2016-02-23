using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Embark.Interaction;
using EmbarkTests._Mocks;

namespace EmbarkTests.Interaction
{
    public class CollectionTests_Range
    {
        [Fact]
        public void GetAll_ReturnsAllItems()
        {
            // arrange
            var allTestCollection = _MockDB.SharedRuntimeClient["SelectAll"];
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
            var allTestCollection = _MockDB.SharedRuntimeClient["SelectBetween"];
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

            _MockDB.RuntimeBasicCollection.Insert(oldWooly);
            _MockDB.RuntimeBasicCollection.Insert(oldDusty);
            _MockDB.RuntimeBasicCollection.Insert(youngLassy);

            // act            

            IEnumerable<Sheep> matchQuery = _MockDB.RuntimeBasicCollection.GetWhere<Sheep>(new { Age = 100 }).Unwrap();

            var ancients = matchQuery.ToList();

            // assert
            Assert.Equal(2, ancients.Count);

            Assert.False(ancients.Any(s => s.Age != 100));
            Assert.False(ancients.Any(s => s.Name == "Lassy"));

            Assert.True(ancients.Any(s => s.Name == "Wooly"));
            Assert.True(ancients.Any(s => s.Name == "Dusty"));
        }

        [Fact]
        public void DeleteAll_EmptiesCollection()
        {
            // arrange
            var delCollection = _MockDB.SharedRuntimeClient["DeleteAll"].AsGenericCollection<Sheep>();
            int testSize = 7;

            foreach (var sheep in Sheep.GetTestHerd(testSize))
                delCollection.Insert(sheep);

            Assert.Equal(testSize, delCollection.GetAll().Count());

            // act
            var deleted = delCollection.DeleteAll();
            var reDeleted = delCollection.DeleteAll();

            // assert
            Assert.Equal(0, delCollection.GetAll().Count());

            Assert.Equal(testSize, deleted);
            Assert.Equal(0, reDeleted);
        }
    }
}
