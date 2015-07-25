using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClient.TestData;
using TestClient.TestData.DataEntry;
using System.Collections.Generic;
using System.Linq;

namespace TestClient
{
    [TestClass]
    public class DataEntryRange
    {
        [TestMethod]
        public void GetAll_ReturnsAllDocuments()
        {
            // arrange
            var io = Cache.GetSoundCollection("GetAllTest");
            var created = GenerateTestSounds();

            foreach (var entry in created)
                io.Insert(entry);

            // act
            var loaded = io.GetAll().ToList();

            // assert
            Assert.AreEqual(created.Count, loaded.Count);
        }

        [TestMethod]
        public void GetLike_ReturnsSimilarMatches()
        {
            // arrange
            var io = Cache.GetSoundCollection("GetLikeTest");
            var created = GenerateTestSounds();

            var comparison = created[0];

            var linqLikeCount = created.Count(s =>
                s.Description == comparison.Description &&
                s.Echo.Repetitions == comparison.Echo.Repetitions);

            foreach (var entry in created)
                io.Insert(entry);

            var query = new
            {
                Description = comparison.Description,
                Echo = new { Repetitions = comparison.Echo.Repetitions }
            };

            // act
            var matches = io.GetWhere(query).ToList();

            // assert
            Assert.IsTrue(matches.Count > 0);
            Assert.AreEqual(linqLikeCount, matches.Count);
            Assert.IsTrue(matches.All(m =>
                m.Description == comparison.Description &&
                m.Echo.Repetitions == comparison.Echo.Repetitions));
        }
        
        [TestMethod]
        public void GetBetween_ReturnsMatchesRangeQuery()
        {
            // arrange
            var io = Cache.GetSoundCollection("GetBetweenTest");
            var created = GenerateTestSounds();
            created.Add(new Sound
            {
                Quality = 120,
                Echo = new TestData.Basic.Echo { Repetitions = 5}
            });

            var linqBetweenCount = created.Count(s =>
                100 <= s.Quality && s.Quality <= 150 &&
                3 <= s.Echo.Repetitions && s.Echo.Repetitions <= 7);

            foreach (var entry in created)
                io.Insert(entry);

            var queryStart = new
            {
                Quality = 100,
                Echo = new { Repetitions = 3 }
            };

            var queryEnd = new
            {
                Quality = 150,
                Echo = new { Repetitions = 7 }
            };

            // act
            var matches = io.GetBetween(queryStart, queryEnd).ToList();
            
            // assert
            Assert.IsTrue(matches.Count > 0);
            Assert.AreEqual(linqBetweenCount, matches.Count);
            Assert.IsTrue(matches.All(m =>
                100 <= m.Quality && m.Quality <= 150 &&
                3 <= m.Echo.Repetitions && m.Echo.Repetitions <= 7));
        }

        private static List<Sound> GenerateTestSounds(int size = 5)
        {
            return Enumerable.Range(0, size)
                .Select(i => TestEntities.GetTestSound(i))
                .ToList();
        }
    }
}
