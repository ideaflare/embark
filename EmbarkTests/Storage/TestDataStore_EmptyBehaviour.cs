using Xunit;
using System.Linq;
using Embark;

namespace EmbarkTests.Storage
{
    public class TestDataStore_EmptyBehaviour
    {
        [Fact]
        public void DeleteNothing_ReturnsFalse()
        {
            using (var testDB = MockDB.GetDiskDB())
            {
                var diskClient = testDB.GetNewLocalDB;
                var runtimeClient = Client.GetRuntimeDB();

                DeleteNothing_ReturnsFalse(diskClient);
                DeleteNothing_ReturnsFalse(runtimeClient);
            }
        }

        private static void DeleteNothing_ReturnsFalse(Client client)
        {
            bool deletedSomething = client["test"].Delete(-3);
            Assert.False(deletedSomething);
        }

        [Fact]
        public void GetNothing_ReturnsNothing()
        {
            using (var testDB = MockDB.GetDiskDB())
            {
                var diskClient = testDB.GetNewLocalDB;
                var runtimeClient = Client.GetRuntimeDB();

                GetNothing_ReturnsNothing(diskClient);
                GetNothing_ReturnsNothing(runtimeClient);
            }
        }

        private static void GetNothing_ReturnsNothing(Client client)
        {
            var noItem = client["a"].Get<object>(7);
            var allItems = client["b"].GetAll<object>().Count();

            Assert.Equal(null, noItem);
            Assert.Equal(0, allItems);
        }
    }
}