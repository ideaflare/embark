using Embark;
using Xunit;
using System.Linq;

namespace EmbarkTests.Storage
{
    public class TestRuntimeDataStoreNegative
    {
        [Fact]
        public void DeleteNothing_False()
        {
            var tempClient = Client.GetRuntimeDB();

            bool deletedSomething = tempClient["test"].Delete(-3);

            Assert.False(deletedSomething);
        }

        [Fact]
        public void GetNothing_ReturnsNothing()
        {
            // arrange
            var collection = Client.GetRuntimeDB();

            // act
            var noItem = collection["a"].Get<object>(7);
            var allItems = collection["b"].GetAll<object>().Count();

            // assert
            Assert.Equal(null, noItem);
            Assert.Equal(0, allItems);
        }
    }
}