using Embark;
using Embark.Interaction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace EmbarkTests
{
    [TestClass]
    public class MockDB
    {
        internal static Client IOClient;

        internal static Collection BasicCollection;
        //internal static DataEntryCollection<Sound> DataEntryCollection;

        //private static readonly string testDir = @"C:\MyTemp\EmbarkTests\";

        //internal static DataEntryCollection<Sound> GetSoundCollection(string collectionName)
        //{
        //    return localClient.GetDataEntryCollection<Sound>(collectionName);
        //}

        [AssemblyInitialize()]
        public static void MyTestInitialize(TestContext testContext)
        {
            IOClient = Client.GetRuntimeDB();

            BasicCollection = IOClient.Basic;
            //DataEntryCollection = localClient.GetDataEntryCollection<Sound>("ConventionTests");

            Assert.IsNotNull(BasicCollection);
        }

        [AssemblyCleanup()]
        public static void MytestCleanup()
        {

        }
    }
}
