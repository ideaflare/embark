using Xunit;
using System.Linq;

namespace EmbarkTests.Storage
{
    public class TestDiskDataStoreNegative
    {
        [Fact]
        public void DeleteNothing_False()
        {
            using (var testDB = MockDB.GetDiskDB())
            {
                bool deletedSomething = testDB.TestClient["test"].Delete(-3);
                Assert.False(deletedSomething);
            }
        }

        [Fact]
        public void GetNothing_ReturnsNothing()
        {
            using (var testDB = MockDB.GetDiskDB())
            {
                // arrange
                var client = testDB.TestClient;

                // act
                var noItem = client["a"].Get<object>(7);
                var allItems = client["b"].GetAll<object>().Count();

                // assert
                Assert.Equal(null, noItem);
                Assert.Equal(0, allItems);
            }
        }
    }
}