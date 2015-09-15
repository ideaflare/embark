using EmbarkTests._Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmbarkTests.Interaction
{
    [TestClass]
    public class TestCollection_Basic
    {
        [TestMethod]
        public void Insert_ReturnsID()
        {
            // arrange
            var sheep = Sheep.GetTestSheep();

            // act
            object id = MockDB.RuntimeBasicCollection.Insert(sheep);

            // assert
            Assert.AreEqual(typeof(long), id.GetType());
        }

        [TestMethod]
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
            Assert.IsTrue(hasSheepUpdated);
            Assert.AreEqual(updated.Name, loaded.Name);
            Assert.AreEqual(updated.Age, loaded.Age);
            Assert.AreEqual(updated.FavouriteIceCream, loaded.FavouriteIceCream);
            Assert.AreNotEqual(source.Age, updated.Age);
        }

        [TestMethod]
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
            Assert.IsTrue(hasSheepVanished);
            Assert.IsNotNull(source);
            Assert.IsNull(loaded);
            Assert.IsFalse(updateDeletedSheep);
        }

        [TestMethod]
        public void Get_RetrievesSheep()
        {
            // arrange
            var saved = Sheep.GetTestSheep();

            long id = MockDB.RuntimeBasicCollection.Insert(saved);

            // act
            Sheep loaded = MockDB.RuntimeBasicCollection.Get<Sheep>(id);

            // assert
            Assert.AreEqual(saved.Name, loaded.Name);
            Assert.AreEqual(saved.Age, loaded.Age);
            Assert.AreEqual(saved.FavouriteIceCream, loaded.FavouriteIceCream);
        }
    }
}
