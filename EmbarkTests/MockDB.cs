using Embark;
using Embark.Interaction;
using EmbarkTests._Mocks;
using Xunit;
using System;
using System.IO;
using System.Reflection;

namespace EmbarkTests
{
    
    public class MockDB
    {
        internal static Client SharedRuntimeClient = SetupEnvironmentAndGetTestClient();
        //internal static Client SharedDiskClient;

        internal static Collection RuntimeBasicCollection;
        internal static DataEntryCollection<Sound> RuntimeDataEntryCollection;

        internal static DataEntryCollection<Sound> GetRuntimeCollection(string collectionName)
        {
            return SharedRuntimeClient.GetDataEntryCollection<Sound>(collectionName);
        }

        static Client SetupEnvironmentAndGetTestClient()
        {
            SharedRuntimeClient = Client.GetRuntimeDB();

            RuntimeBasicCollection = SharedRuntimeClient.Basic;
            RuntimeDataEntryCollection = SharedRuntimeClient.GetDataEntryCollection<Sound>("ConventionTests");

            Assert.NotNull(RuntimeBasicCollection);

            return SharedRuntimeClient;
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
