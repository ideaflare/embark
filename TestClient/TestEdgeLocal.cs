using Embark;
using Embark.Conversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.TestData;

namespace TestClient
{
    [TestClass]
    public class TestEdgeLocal
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Only alphanumerical & underscore characters supported in collection names.")]
        public void CollectionName_OnlyAlphanumericAndUnderScoreSupported()
        {
            var na = Client.GetLocalDB()["!?$@\filesystem.*"];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException),"Collection name should be at least one alphanumerical or underscore character.")]
        public void CollectionName_CannotBeEmpty()
        {
            var na = Client.GetLocalDB()[""];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Collection name should be at least one alphanumerical or underscore character.")]
        public void CollectionName_CannotBeNull()
        {
            var na = Client.GetLocalDB()[null];
        }
        
        [TestMethod]
        public void SaveBlob_CanDeserializeToByteArray()
        {
            // arrange
            byte[] savedData = new byte[64];
            (new Random()).NextBytes(savedData);

            var saved = new { blob = savedData };

            long id = Cache.localCache.Basic.Insert(saved);

            // act
            var loaded = Cache.localCache.Basic.Select<Dictionary<string,object>>(id);
            var blob = loaded["blob"];
            byte[] loadedData = ExtensionMethods.GetByteArray(blob);

            // assert
            Assert.IsTrue(Enumerable.SequenceEqual(savedData, loadedData));
        }
                
        //[TestMethod]
        public void UpdateNonExisting_ReturnsFalse()
        {
            throw new NotImplementedException();
        }
    }
}
