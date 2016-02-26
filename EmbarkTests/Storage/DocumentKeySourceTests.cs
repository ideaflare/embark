using Xunit;
using System;
using System.Linq;

namespace EmbarkTests.StorageTests
{
    public class DocumentKeySourceTests
    {
        [Fact]
        public void NewIDs_AreUnique()
        {
            int totalInserts = 500;
            var db = Embark.Client.GetRuntimeDB().Basic;

            var uniqueIDs = Enumerable.Range(0, totalInserts)
                .AsParallel()
                .Select(i => db.Insert(new { n = 0 }))
                .Distinct()
                .Count();

            Assert.Equal(totalInserts, uniqueIDs);
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
