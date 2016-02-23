using Xunit;
using System;
using System.Diagnostics;
using System.Linq;

namespace EmbarkTests.StorageTests
{
    public class TestDocumentKeySource
    {
        // TODO split performance test & parallel generation test.
        [Fact]
        public void NewIDs_AreUnique()
        {
            int totalInserts = 15;
            double timePerInsert = 50;// milliseconds per insert. 
            // Test written on laptop with Samsung 840 SSD. Increase insert time if machine uses a spinning magnetic relic.

            var db = Embark.Client.GetRuntimeDB().Basic;

            // insert IDs in parallel
            var sw = Stopwatch.StartNew();

            var newIDs = Enumerable.Range(0, totalInserts)
                .AsParallel()
                .Select(i => db.Insert(new { n = 0 }))
                .ToList();

            sw.Stop();

            // test that they are unique
            Assert.Equal(totalInserts, newIDs.Distinct().Count());
            // and completed within average timePerInsert time
            Assert.True(sw.ElapsedMilliseconds < timePerInsert * totalInserts);
        }

        [Fact]
        public void CreatedID_IsTimeStamp()
        {
            // arrange
            var now = DateTime.Now;
            long id = _MockDB.RuntimeBasicCollection.Insert(new { Numero = "Uno" });

            // act
            var timestamp = new DateTime(id);

            // assert
            var timeDiff = timestamp.Subtract(now);
            Assert.True(timeDiff.TotalSeconds < 1);
        }
    }
}
