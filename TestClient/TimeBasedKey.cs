using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Diagnostics;
using TestClient.TestData;

namespace TestClient
{
    [TestClass]
    public class TimeBasedKey
    {
        // TODO test in RAM only, don't hit disk, increase parallel totalInserts to 100.
        [TestMethod]
        public void NewIDs_AreUnique()
        {
            int totalInserts = 15;
            double timePerInsert = 20;// milliseconds per insert. 
            // Test written on laptop with Samsung 840 SSD. Increase insert time if machine uses a spinning magnetic relic.

            // insert IDs in parallel
            var sw = Stopwatch.StartNew();
            var newIDs = Enumerable.Range(0, totalInserts)
                .AsParallel()
                .Select(i => Cache.BasicCollection.Insert(new { n = 0 }))
                .ToList();
            sw.Stop();

            // test that they are unique
            Assert.AreEqual(totalInserts, newIDs.Distinct().Count());
            // and completed within average timePerInsert time
            Assert.IsTrue(sw.ElapsedMilliseconds < timePerInsert * totalInserts);
        }

        [TestMethod]
        public void CreatedID_IsTimeStamp()
        {
            // arrange
            var now = DateTime.Now;
            long id = Cache.BasicCollection.Insert(new { Numero = "Uno" });

            // act
            var timestamp = new DateTime(id);

            // assert
            var timeDiff = timestamp.Subtract(now);
            Assert.IsTrue(timeDiff.TotalSeconds < 1);
        }
                       

        //[TestMethod]
        public void TestObject_IsRevarsableSerializable()
        {
            throw new NotImplementedException();
        }
    }
}
