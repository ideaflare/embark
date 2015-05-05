using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClient.TestData;

namespace TestClient
{
    [TestClass]
    public class DataEntryRange
    {

        [TestMethod]
        public void GetAll_ReturnsAllDocuments()
        {
            // arrange
            var collection = Cache.GetSoundCollection("GetAllTest");
            var testData = TestEntities.GetTestSound();

            // act

            // assert

            throw new NotImplementedException();
        }


        [TestMethod]
        public void GetLike_ReturnsSimilarMatches()
        {
            throw new NotImplementedException();
        }


        [TestMethod]
        public void GetBetween_ReturnsMatchesRangeQuery()
        {
            throw new NotImplementedException();
        }

    }
}
