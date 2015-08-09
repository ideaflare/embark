using EmbarkTests._Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbarkTests.Interaction
{
    [TestClass]
    public class CollectionTests
    {
        [TestMethod]
        public void Insert_ReturnsID()
        {
            // arrange
            var sheep = Sheep.GetTestSheep();

            // act
            object id = MockDB.BasicCollection.Insert(sheep);

            // assert
            Assert.AreEqual(typeof(long), id.GetType());
        }

        [TestMethod]
        public void Update_ModifiesSheep()
        {
            // arrange
            var saved = Sheep.GetTestSheep();

            long id = MockDB.BasicCollection.Insert(saved);

            Sheep source = MockDB.BasicCollection.Get<Sheep>(id);

            Sheep updated = new Sheep { Name = source.Name, FavouriteIceCream = source.FavouriteIceCream };
            updated.Age = source.Age + 1;

            // act            
            bool hasSheepUpdated = MockDB.BasicCollection.Update(id, updated);
            Sheep loaded = MockDB.BasicCollection.Get<Sheep>(id);

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

            long id = MockDB.BasicCollection.Insert(saved);

            Sheep source = MockDB.BasicCollection.Get<Sheep>(id);

            // act            
            bool hasSheepVanished = MockDB.BasicCollection.Delete(id);
            Sheep loaded = MockDB.BasicCollection.Get<Sheep>(id);
            var updateDeletedSheep = MockDB.BasicCollection.Update<Sheep>(id, null);

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

            long id = MockDB.BasicCollection.Insert(saved);

            // act
            Sheep loaded = MockDB.BasicCollection.Get<Sheep>(id);

            // assert
            Assert.AreEqual(saved.Name, loaded.Name);
            Assert.AreEqual(saved.Age, loaded.Age);
            Assert.AreEqual(saved.FavouriteIceCream, loaded.FavouriteIceCream);
        }
    }
}
