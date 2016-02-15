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

        internal static TestDiskDB GetDiskDB() => new TestDiskDB();

        static Client SetupEnvironmentAndGetTestClient()
        {
            SharedRuntimeClient = Client.GetRuntimeDB();

            RuntimeBasicCollection = SharedRuntimeClient.Basic;
            RuntimeDataEntryCollection = SharedRuntimeClient.GetDataEntryCollection<Sound>("ConventionTests");

            Assert.NotNull(RuntimeBasicCollection);

            return SharedRuntimeClient;
        }

        public sealed class TestDiskDB : IDisposable
        {
            public Client TestClient { get; private set; }
            private string testDir;

            public TestDiskDB()
            {
                testDir = $"{ AssemblyDirectory }\\Embark_Test_Temp_{ Guid.NewGuid() }\\";

                Directory.CreateDirectory(testDir);

                TestClient = Client.GetLocalDB(testDir);
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


            public void Dispose()
            {
                Directory.Delete(testDir, recursive: true);
            }
        }
    }
}
