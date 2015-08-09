using Embark;
using Embark.Interaction;
using EmbarkTests._Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace EmbarkTests
{
    [TestClass]
    public class MockDB
    {
        internal static Client SharedClient;

        internal static Collection BasicCollection;
        internal static DataEntryCollection<Sound> DataEntryCollection;

        internal static DataEntryCollection<Sound> GetSoundCollection(string collectionName)
        {
            return SharedClient.GetDataEntryCollection<Sound>(collectionName);
        }

        [AssemblyInitialize()]
        public static void MyTestInitialize(TestContext testContext)
        {
            SharedClient = Client.GetRuntimeDB();

            BasicCollection = SharedClient.Basic;
            DataEntryCollection = SharedClient.GetDataEntryCollection<Sound>("ConventionTests");

            Assert.IsNotNull(BasicCollection);
        }

        [AssemblyCleanup()]
        public static void MytestCleanup()
        {

        }
    }
}
