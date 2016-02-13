using EmbarkTests._Mocks;
using Xunit;

namespace EmbarkTests.Interaction
{
    public class TestCollection_Basic
    {
        [Fact]
        public void Insert_ReturnsID()
        {
            // arrange
            var sheep = Sheep.GetTestSheep();

            // act
            object id = MockDB.RuntimeBasicCollection.Insert(sheep);

            // assert
            Assert.Equal(typeof(long), id.GetType());
        }

        [Fact]
        public void Update_ModifiesSheep()
        {
            // arrange
            var saved = Sheep.GetTestSheep();

            long id = MockDB.RuntimeBasicCollection.Insert(saved);

            Sheep source = MockDB.RuntimeBasicCollection.Get<Sheep>(id);

            Sheep updated = new Sheep { Name = source.Name, FavouriteIceCream = source.FavouriteIceCream };
            updated.Age = source.Age + 1;

            // act            
            bool hasSheepUpdated = MockDB.RuntimeBasicCollection.Update(id, updated);
            Sheep loaded = MockDB.RuntimeBasicCollection.Get<Sheep>(id);

            // assert
            Assert.True(hasSheepUpdated);
            Assert.Equal(updated.Name, loaded.Name);
            Assert.Equal(updated.Age, loaded.Age);
            Assert.Equal(updated.FavouriteIceCream, loaded.FavouriteIceCream);
            Assert.NotEqual(source.Age, updated.Age);
        }

        [Fact]
        public void Delete_MakesSheepVanish()
        {
            // arrange
            var saved = Sheep.GetTestSheep();

            long id = MockDB.RuntimeBasicCollection.Insert(saved);

            Sheep source = MockDB.RuntimeBasicCollection.Get<Sheep>(id);

            // act            
            bool hasSheepVanished = MockDB.RuntimeBasicCollection.Delete(id);
            Sheep loaded = MockDB.RuntimeBasicCollection.Get<Sheep>(id);
            var updateDeletedSheep = MockDB.RuntimeBasicCollection.Update<Sheep>(id, null);

            // assert
            Assert.True(hasSheepVanished);
            Assert.NotNull(source);
            Assert.Null(loaded);
            Assert.False(updateDeletedSheep);
        }

        [Fact]
        public void Get_RetrievesSheep()
        {
            // arrange
            var saved = Sheep.GetTestSheep();

            long id = MockDB.RuntimeBasicCollection.Insert(saved);

            // act
            Sheep loaded = MockDB.RuntimeBasicCollection.Get<Sheep>(id);

            // assert
            Assert.Equal(saved.Name, loaded.Name);
            Assert.Equal(saved.Age, loaded.Age);
            Assert.Equal(saved.FavouriteIceCream, loaded.FavouriteIceCream);
        }
    }
}
