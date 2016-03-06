using Embark;
using Embark.Interaction;
using EmbarkTests._Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EmbarkTests.TextConversion
{
    public class JavascriptSerializerConverterTests
    {
        private Random random = new Random();

        [Fact]
        public void SerializeRawByteArray_CanDeserializeToByteArray()
        {
            // arrange
            byte[] rawByteArray = new byte[64];
            random.NextBytes(rawByteArray);

            //act
            long idRawByteArray = _MockDB.SharedRuntimeClient.Basic.Insert(rawByteArray);

            //assert
            byte[] loadedAsArray = _MockDB.SharedRuntimeClient.Basic.Get<byte[]>(idRawByteArray);

            Assert.True(Enumerable.SequenceEqual(rawByteArray, loadedAsArray));
        }

        [Fact]
        public void SerializePropertyByteArray_CanDeserializeToObjectProperty()
        {
            // arrange
            byte[] rawByteArray = new byte[64];
            random.NextBytes(rawByteArray);

            var sound = new Sound
            {
                Sample = rawByteArray,
                Echo = null
            };

            long idSound = _MockDB.SharedRuntimeClient.Basic.Insert(sound);

            // act    
            var loadedSound = _MockDB.SharedRuntimeClient.Basic.Get<Sound>(idSound);
            byte[] loadedSample = loadedSound.Sample;

            // assert
            Assert.True(Enumerable.SequenceEqual(rawByteArray, loadedSample));
        }               

        [Fact]
        public void SerializedObject_CanDeserializeToOtherType()
        {
            // arrange
            Sheep saved = Sheep.GetTestSheep();

            long id = _MockDB.RuntimeBasicCollection.Insert(saved);

            // act
            Cat loaded = _MockDB.RuntimeBasicCollection.Get<Cat>(id);
            Cat defaultCat = new Cat();

            // assert
            Assert.Equal(loaded.Name, saved.Name);
            Assert.Equal(loaded.Age, saved.Age);
            Assert.Equal(loaded.FurDensity, defaultCat.FurDensity);
            Assert.Equal(loaded.HasMeme, defaultCat.HasMeme);
        }

        // TODO: move test setup into seperate Testfixture and split test
        // -> InlineQuery_SameAsAnonymousQuery
        // -> QueryData_ReturnsExpectedResult
        // -> IsBetweenMatch evaluates sub properties
        // -> Use fixture for other test: "MixedTypeCllection_CanSave" Collection can handle multiple types 
        [Fact]
        public void IsBetweenMatch_EvaluatesSubProperties()
        {
            // arrange - insert test data
            var oldWooly = new Sheep { Name = "Wooly", Age = 100, FavouriteIceCream = IceCream.Chocolate };
            var oldDusty = new Sheep { Name = "Dusty", Age = 100, FavouriteIceCream = IceCream.Chocolate, OnTable = new Table { Legs = 2 } };
            var youngLassy = new Sheep { Name = "Lassy", Age = 1, FavouriteIceCream = IceCream.Bubblegum, OnTable = new Table { IsSquare = true } };

            var io = Client.GetRuntimeDB().GetCollection<Sheep>("subMatch");

            long id = io.Insert(oldWooly);
            long id2 = io.Insert(oldDusty);
            long id3 = io.Insert(youngLassy);

            // act - query inserted data
            var anonymousTable = new { Legs = 2 };
            var query = new { Age = 100, OnTable = anonymousTable };
            IEnumerable<Sheep> matchQueryAnonymous = io.GetWhere(query).Unwrap();

            var queryResult = matchQueryAnonymous.ToList();

            var inlineQueryResult = io
                .GetWhere(new { Age = 100, OnTable = new { Legs = 2 } })
                .Unwrap()
                .ToList();

            // assert
            Assert.Equal(1, queryResult.Count);

            Assert.False(queryResult.Any(s => s.Age != 100));
            Assert.False(queryResult.Any(s => s.OnTable.Legs != 2));

            Assert.False(queryResult.Any(s => s.Name == "Lassy"));
            Assert.False(queryResult.Any(s => s.Name == "Wooly"));

            Assert.True(queryResult.Any(s => s.Name == "Dusty"));

            Assert.True(Enumerable.SequenceEqual(inlineQueryResult, queryResult));
        }

        [Fact]
        public void SaveNonPoco_HandlesComparison()
        {
            var io = Client.GetRuntimeDB().GetCollection<string>("nonPOCO");
            string input = "string";

            string inserted = InsertAndGetValue<string>(io, input);

            Assert.Equal(input, inserted);
        }

        private static T InsertAndGetValue<T>(Collection<T> io, T input) where T : class
        {
            var id = io.Insert(input);

            TestRangeCommandsRunWithoutError(io, input);

            return io.Get(id);
        }

        private static void TestRangeCommandsRunWithoutError<T>(Collection<T> io, T input) where T : class
        {
            var all = io.GetAll().ToArray();
            var like = io.GetWhere(input).ToArray();
            var between = io.GetBetween("str", "qqqqing").ToArray();
        }
    }
}
