using Embark;
using System;
using System.IO;
using System.Threading;

namespace EmbarkTests._Mocks
{
    public sealed class TestDiskFolder : IDisposable
    {
        public Client GetNewLocalDB() => Client.GetLocalDB(TestDir);
        public Client GetNewCachedDB(int maxAsyncOperations = int.MaxValue) => Client.GetCachedDB(TestDir, maxAsyncOperations: maxAsyncOperations);

        public string TestDir { get; }

        public TestDiskFolder()
        {
            TestDir = $"{ AssemblyDirectory }\\Embark_Test_Temp_{ Guid.NewGuid() }\\";

            Directory.CreateDirectory(TestDir);
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
            int retries = 0;
            while (Directory.Exists(TestDir))
            {
                try
                {
                    Directory.Delete(TestDir, recursive: true);
                }
                catch (Exception)
                {
                    if (retries > 10)
                        throw;
                    Thread.Sleep(++retries * 100);
                }
            }
        }
    }
}
