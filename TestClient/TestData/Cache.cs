using Embark;
using Embark.Interaction;
using Embark.TextConversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestClient.TestData.DataEntry;

namespace TestClient.TestData
{
    [TestClass]
    public class Cache
    {
        internal static Client localClient;

        internal static Collection BasicCollection;
        internal static DataEntryCollection<Sound> DataEntryCollection;

        private const string testDir = @"C:\MyTemp\EmbarkTests\";

        internal static DataEntryCollection<Sound> GetSoundCollection(string collectionName)
        {
            return localClient.GetDataEntryCollection<Sound>(collectionName);
        }

        [AssemblyInitialize()]
        public static void MyTestInitialize(TestContext testContext)
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, recursive: true);

            Directory.CreateDirectory(testDir);

            localClient = Client.GetLocalDB(testDir);

            BasicCollection = localClient.Basic;
            DataEntryCollection = localClient.GetDataEntryCollection<Sound>("ConventionTests");

            Assert.IsNotNull(BasicCollection);
            Assert.IsNotNull(DataEntryCollection);
        }

        [AssemblyCleanup()]
        public static void MytestCleanup()
        {
            if (Directory.Exists(testDir))
                Directory.Delete(testDir, recursive: true);
        }
    }
}