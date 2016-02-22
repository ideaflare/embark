using Embark;
using System;
using System.IO;

namespace EmbarkTests._Mocks
{
    public sealed class TestDiskDB : IDisposable
    {
        public Client GetNewLocalDB() => Client.GetLocalDB(testDir);
        public Client GetNewCachedDB() => Client.GetCachedDB(testDir);

        private string testDir;

        public TestDiskDB()
        {
            testDir = $"{ AssemblyDirectory }\\Embark_Test_Temp_{ Guid.NewGuid() }\\";

            Directory.CreateDirectory(testDir);
        }

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
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
