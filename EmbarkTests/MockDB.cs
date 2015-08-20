using Embark;
using Embark.Interaction;
using EmbarkTests._Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;

namespace EmbarkTests
{
    [TestClass]
    public class MockDB
    {
        internal static Client SharedRuntimeClient;
        //internal static Client SharedDiskClient;

        internal static Collection RuntimeBasicCollection;
        internal static DataEntryCollection<Sound> RuntimeDataEntryCollection;

        internal static DataEntryCollection<Sound> GetRuntimeCollection(string collectionName)
        {
            return SharedRuntimeClient.GetDataEntryCollection<Sound>(collectionName);
        }

        [AssemblyInitialize()]
        public static void MyTestInitialize(TestContext testContext)
        {
            SharedRuntimeClient = Client.GetRuntimeDB();

            RuntimeBasicCollection = SharedRuntimeClient.Basic;
            RuntimeDataEntryCollection = SharedRuntimeClient.GetDataEntryCollection<Sound>("ConventionTests");

            Assert.IsNotNull(RuntimeBasicCollection);
        }

        [AssemblyCleanup()]
        public static void MytestCleanup()
        {

        }

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
