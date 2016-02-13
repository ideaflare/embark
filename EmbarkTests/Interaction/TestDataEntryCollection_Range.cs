using EmbarkTests._Mocks;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace EmbarkTests.Interaction
{
    public class TestDataEntryCollection_Range
    {
        [Fact]
        public void GetAll_ReturnsAllDocuments()
        {
            // arrange
            var io = MockDB.GetRuntimeCollection("GetAllTest");
            var created = GenerateTestSounds();

            foreach (var entry in created)
                io.Insert(entry);

            // act
            var loaded = io.GetAll().ToList();

            // assert
            Assert.Equal(created.Count, loaded.Count);
        }

        [Fact]
        public void GetLike_ReturnsSimilarMatches()
        {
            // arrange
            var io = MockDB.GetRuntimeCollection("GetLikeTest");
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
            Assert.True(matches.Count > 0);
            Assert.Equal(linqLikeCount, matches.Count);
            Assert.True(matches.All(m =>
                m.Description == comparison.Description &&
                m.Echo.Repetitions == comparison.Echo.Repetitions));
        }

        [Fact]
        public void GetBetween_ReturnsMatchesRangeQuery()
        {
            // arrange
            var io = MockDB.GetRuntimeCollection("GetBetweenTest");
            var created = GenerateTestSounds();
            created.Add(new Sound
            {
                Quality = 120,
                Echo = new Echo { Repetitions = 5 }
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
            Assert.True(matches.Count > 0);
            Assert.Equal(linqBetweenCount, matches.Count);
            Assert.True(matches.All(m =>
                100 <= m.Quality && m.Quality <= 150 &&
                3 <= m.Echo.Repetitions && m.Echo.Repetitions <= 7));
        }

        private static List<Sound> GenerateTestSounds(int size = 5)
        {
            return Enumerable.Range(0, size)
                .Select(i => Sound.GetTestSound(i))
                .ToList();
        }
    }
}
