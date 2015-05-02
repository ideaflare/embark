using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClient.IO;
using System.IO;
using Embark;
using TestClient.IO.TestData;
using TestClient.TestData;

namespace TestClient
{
    [TestClass]
    public class BasicCommands
    {        
        [TestMethod]
        public void Insert_ReturnsID()
        {
            // arrange
            var sheep = TestEntities.GetTestSheep();

            // act
            object id = Cache.localSheep.Insert(sheep);

            // assert
            Assert.AreEqual(typeof(long), id.GetType());
        }

        [TestMethod]
        public void Get_RetrievesSheep()
        {
            // arrange
            var saved = TestEntities.GetTestSheep();
                       
            long id = Cache.localSheep.Insert(saved);

            // act
            Sheep loaded = Cache.localSheep.Get<Sheep>(id);

            // assert
            Assert.AreEqual(saved.Name, loaded.Name);
            Assert.AreEqual(saved.Age, loaded.Age);
            Assert.AreEqual(saved.FavouriteIceCream, loaded.FavouriteIceCream);
        }
        

        [TestMethod]
        public void Update_ModifiesSheep()
        {
            // arrange
            var saved = TestEntities.GetTestSheep();

            long id = Cache.localSheep.Insert(saved);

            Sheep source = Cache.localSheep.Get<Sheep>(id);

            Sheep updated = new Sheep { Name = source.Name, FavouriteIceCream = source.FavouriteIceCream };
            updated.Age = source.Age + 1;
            
            // act            
            bool hasSheepUpdated = Cache.localSheep.Update(id, updated);
            Sheep loaded = Cache.localSheep.Get<Sheep>(id);

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
            var saved = TestEntities.GetTestSheep();

            long id = Cache.localSheep.Insert(saved);

            Sheep source = Cache.localSheep.Get<Sheep>(id);

            // act            
            bool hasSheepVanished = Cache.localSheep.Delete(id);
            Sheep loaded = Cache.localSheep.Get<Sheep>(id);

            // assert
            Assert.IsTrue(hasSheepVanished);
            Assert.IsNotNull(source);
            Assert.IsNull(loaded);
        }

        

    }
}
