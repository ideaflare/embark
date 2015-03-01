using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClient.IO;

namespace TestClient
{
    [TestClass]
    public class BasicIOTests
    {
        private static long SaveNewSheep()
        {
            var sheep = TestObjects.GetTestSheep();
            var tag = "sheep";

            long id = EmbarkClient.Gateway.Insert(tag, sheep);

            return id;
        }

        //basic
        [TestMethod]
        public void Create_ReturnsID()
        {
            object id = SaveNewSheep();
            Assert.AreEqual(typeof(long), id.GetType());
        }

        [TestMethod]
        public void Read_SpecificSheepIsSheep()
        {

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
            long id = SaveNewSheep();

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
