using EmbarkTests._Mocks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EmbarkTests.Storage
{
    public class CachedDataStoreTests
    {
        [Fact]
        public void CachedStoreInserts_10xTimesFasterThanDiskStore()
        {
            // arrange
            int testSize = 100;

            using (var diskDB = _MockDB.GetDiskDB())
            using (var cachedDB = _MockDB.GetDiskDB())
            {
                var testItems = _Mocks.Sheep.GetTestHerd(testSize);
                
                var cachedClient = cachedDB.GetNewCachedDB();
                var cachedCollection = cachedClient.GetCollection<Sheep>("na");

                var diskCollection = diskDB.GetNewLocalDB().GetCollection<Sheep>("na");
                
                // act
                var swTimeDisk = Stopwatch.StartNew();
                foreach (var s in testItems)
                    diskCollection.Insert(s);
                swTimeDisk.Stop();

                var swCacheDisk = Stopwatch.StartNew();
                foreach (var s in testItems)
                    cachedCollection.Insert(s);
                swCacheDisk.Stop();

                // assert

                Assert.True(swTimeDisk.ElapsedMilliseconds > (10 * swCacheDisk.ElapsedMilliseconds));

                // cleanup

                // Wait for caching to complete..
                // TODO call complete caching via api.
                while (true)
                {
                    var persistedCacheCount = Directory.GetFiles(cachedDB.TestDir, "*.*", SearchOption.AllDirectories).Count();

                    if (persistedCacheCount == testSize)
                        break;
                       
                    Thread.Sleep(100);
                }
            }
        }
    }
}
