using Embark;
using Embark.Interaction;
using EmbarkTests._Mocks;
using System.Linq;
using Xunit;

namespace EmbarkTests.TextConversion
{
    class JavascriptSerializerConverterTests
    {
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
