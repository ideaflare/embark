using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClient.IO;

namespace TestClient
{
    //[TestClass]
    public class TestFunctionality
    {
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
            var sheep = Animals.GetTestSheep();
            var tag = "sheep";

            long id = Channel.localCache.Insert(tag, sheep);

            Cat cat = Channel.localCache.Get<Cat>(tag, id);

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
