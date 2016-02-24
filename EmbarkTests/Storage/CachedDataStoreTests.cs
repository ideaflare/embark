using EmbarkTests._Mocks;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;

namespace EmbarkTests.Storage
{
    public class CachedDataStoreTests
    {
        [Fact]
        public void CachedStoreInserts_IsTimesFasterThanDiskStore()
        {
            // arrange
            int testSize = 100;

            using (var diskFolder = _MockDB.GetTestDiskFolder())
            using (var cachedFolder = _MockDB.GetTestDiskFolder())
            {
                var testItems = Sheep.GetTestHerd(testSize);

                var cacheClient = cachedFolder.GetNewCachedDB();
                var diskClient = diskFolder.GetNewLocalDB();

                var cachedCollectionA = cacheClient.GetCollection<Sheep>("A");
                var diskCollectionA = diskClient.GetCollection<Sheep>("A");

                //act
                var swCache = new Stopwatch();
                var swDisk = new Stopwatch();
                foreach(var s in testItems)
                {
                    swCache.Start();
                    cachedCollectionA.Insert(s);
                    swCache.Stop();

                    swDisk.Start();
                    diskCollectionA.Insert(s);
                    swDisk.Stop();
                }

                // assert

                Assert.True(swDisk.ElapsedMilliseconds > swCache.ElapsedMilliseconds);

                // cleanup
                var swCleanup = Stopwatch.StartNew();
                // Wait for caching to complete..
                // TODO call complete caching via api.
                while (true)
                {
                    var persistedCacheCount = Directory.GetFiles(cachedFolder.TestDir, "*.*", SearchOption.AllDirectories).Count();

                    if (persistedCacheCount == testSize)
                        break;

                    Thread.Sleep(100);
                }

                swCleanup.Stop();
                var test = swCleanup.ElapsedMilliseconds;
            }
        }
    }
}
