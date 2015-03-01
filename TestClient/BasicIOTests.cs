using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClient.IO;
using EmbarkClient;

namespace TestClient
{
    [TestClass]
    public class BasicIOTests
    {
        //basic
        [TestMethod]
        public void Create_ReturnsID()
        {
            var sheep = TestObjects.GetTestSheep();
            var tag = "sheep";

            object id = Port.Insert(tag, sheep);

            Assert.AreEqual(typeof(long), id.GetType());
        }

        [TestMethod]
        public void GetSavedSheep_IsSameSheep()
        {
            var saved = TestObjects.GetTestSheep();
            var tag = "sheep";

            long id = Port.Insert(tag, saved);

            Sheep loaded = Port.Get<Sheep>(tag, id);

            Assert.AreEqual(saved.Name, loaded.Name);
            Assert.AreEqual(saved.Age, loaded.Age);
            Assert.AreEqual(saved.FavouriteIceCream, loaded.FavouriteIceCream);
        }
        

        [TestMethod]
        public void Read_AllSheepHasSheep()
        {

            //Arrange

            //Act

            //Assert

        }

        [TestMethod]
        public void ReadSheep_IsSheep()
        {

        }

        //operational
        [TestMethod]
        public void NewIDs_AreUnique()
        {
            //get a couple of hundreds IDs in parallel

            //test that they are unique

            //and completed under a second

            //try to load some previous test ids
            // if there are any, test that the new id's are not in them

            //save new id's to previous id's
            throw new NotImplementedException();
        }
        
        [TestMethod]
        public void Sheep_CanTurnIntoACat()
        {
            var sheep = TestObjects.GetTestSheep();
            var tag = "sheep";

            long id = Port.Insert(tag, sheep);

            Cat cat = Port.Get<Cat>(tag, id);

            Assert.AreEqual(cat.Name, sheep.Name);
            Assert.AreEqual(cat.Age, sheep.Age);
            Assert.AreEqual(cat.FurDensity, (new Cat()).FurDensity);
            Assert.AreEqual(cat.HasMeme, (new Cat()).HasMeme);
        }

        //extra
        [TestMethod]
        public void CreatedID_IsTimeStamp()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void CanInsert_ThousandPerSecond()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void TestObject_IsRevarsableSerializable()
        {
            throw new NotImplementedException();
        }

    }
}
