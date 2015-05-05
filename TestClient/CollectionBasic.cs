using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Embark;
using TestClient.TestData;
using TestClient.TestData.Basic;

namespace TestClient
{
    [TestClass]
    public class CollectionBasic
    {        
        [TestMethod]
        public void Insert_ReturnsID()
        {
            // arrange
            var sheep = TestEntities.GetTestSheep();

            // act
            object id = Cache.BasicCollection.Insert(sheep);

            // assert
            Assert.AreEqual(typeof(long), id.GetType());
        }

        [TestMethod]
        public void Get_RetrievesSheep()
        {
            // arrange
            var saved = TestEntities.GetTestSheep();
                       
            long id = Cache.BasicCollection.Insert(saved);

            // act
            Sheep loaded = Cache.BasicCollection.Get<Sheep>(id);

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

            long id = Cache.BasicCollection.Insert(saved);

            Sheep source = Cache.BasicCollection.Get<Sheep>(id);

            Sheep updated = new Sheep { Name = source.Name, FavouriteIceCream = source.FavouriteIceCream };
            updated.Age = source.Age + 1;
            
            // act            
            bool hasSheepUpdated = Cache.BasicCollection.Update(id, updated);
            Sheep loaded = Cache.BasicCollection.Get<Sheep>(id);

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

            long id = Cache.BasicCollection.Insert(saved);

            Sheep source = Cache.BasicCollection.Get<Sheep>(id);

            // act            
            bool hasSheepVanished = Cache.BasicCollection.Delete(id);
            Sheep loaded = Cache.BasicCollection.Get<Sheep>(id);

            // assert
            Assert.IsTrue(hasSheepVanished);
            Assert.IsNotNull(source);
            Assert.IsNull(loaded);
        }

        

    }
}
