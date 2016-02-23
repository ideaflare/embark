using EmbarkTests._Mocks;
using Xunit;

namespace EmbarkTests.Interaction
{
    public class CollectionTests_Basic
    {
        [Fact]
        public void Insert_ReturnsID()
        {
            // arrange
            var sheep = Sheep.GetTestSheep();

            // act
            object id = _MockDB.RuntimeBasicCollection.Insert(sheep);

            // assert
            Assert.Equal(typeof(long), id.GetType());
        }

        [Fact]
        public void Update_ModifiesSheep()
        {
            // arrange
            var saved = Sheep.GetTestSheep();

            long id = _MockDB.RuntimeBasicCollection.Insert(saved);

            Sheep source = _MockDB.RuntimeBasicCollection.Get<Sheep>(id);

            Sheep updated = new Sheep { Name = source.Name, FavouriteIceCream = source.FavouriteIceCream };
            updated.Age = source.Age + 1;

            // act            
            bool hasSheepUpdated = _MockDB.RuntimeBasicCollection.Update(id, updated);
            Sheep loaded = _MockDB.RuntimeBasicCollection.Get<Sheep>(id);

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

            long id = _MockDB.RuntimeBasicCollection.Insert(saved);

            Sheep source = _MockDB.RuntimeBasicCollection.Get<Sheep>(id);

            // act            
            bool hasSheepVanished = _MockDB.RuntimeBasicCollection.Delete(id);
            Sheep loaded = _MockDB.RuntimeBasicCollection.Get<Sheep>(id);
            var updateDeletedSheep = _MockDB.RuntimeBasicCollection.Update<Sheep>(id, null);

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

            long id = _MockDB.RuntimeBasicCollection.Insert(saved);

            // act
            Sheep loaded = _MockDB.RuntimeBasicCollection.Get<Sheep>(id);

            // assert
            Assert.Equal(saved.Name, loaded.Name);
            Assert.Equal(saved.Age, loaded.Age);
            Assert.Equal(saved.FavouriteIceCream, loaded.FavouriteIceCream);
        }
    }
}
