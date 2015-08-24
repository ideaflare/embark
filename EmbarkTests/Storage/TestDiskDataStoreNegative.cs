using Microsoft.VisualStudio.TestTools.UnitTesting;
using Embark;
using System.Linq;

namespace EmbarkTests.Storage
{
    [TestClass]
    public class TestDiskDataStoreNegative
    {
        [TestMethod]
        public void DeleteNothing_IsFalse()
        {
            var tempClient = Client.GetRuntimeDB();

            bool deletedSomething = tempClient["test"].Delete(-3);

            Assert.IsFalse(deletedSomething);
        }

        [TestMethod]
        public void GetNothing_ReturnsNothing()
        {
            // arrange
            var collection = Client.GetRuntimeDB();

            // act
            var noItem = collection["a"].Get<object>(7);
            var allItems = collection["b"].GetAll<object>().Count();

            // assert
            Assert.AreEqual(null, noItem);
            Assert.AreEqual(0, allItems);
        }
    }
}