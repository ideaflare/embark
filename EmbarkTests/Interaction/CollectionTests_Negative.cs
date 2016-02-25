using EmbarkTests._Mocks;
using Embark;
using System;
using Xunit;

namespace EmbarkTests.Interaction
{
    public class CollectionTests_Negative
    {
        [Fact]
        public void CollectionName_OnlyAlphanumericAndUnderScoreSupported()
        {
            string invalidCharacterMessage = "Only alphanumerical & underscore characters supported in collection names.";

            var err = Assert.Throws<NotSupportedException>(() =>
            {
                var na = Client.GetRuntimeDB()["!?$@\filesystem.*"];
            });

            Assert.Equal(invalidCharacterMessage, err.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CollectionName_CannotBeNullOrEmpty(string collectionName)
        {
            string invalidLengthMessage = "Collection name should be at least one alphanumerical or underscore character.";

            var err = Assert.Throws<ArgumentException>(() =>
            {
                var na = Client.GetRuntimeDB()[collectionName];
            });

            Assert.Equal(invalidLengthMessage, err.Message);
        }

        [Fact]
        public void GetNonExisting_ReturnsNull()
        {
            // arrange
            var client = _MockDB.SharedRuntimeClient;
            var ioBasic = client["basicNonExist"];
            var ioClass = client.GetCollection<Sheep>("genericClassNonExist");
            var ioValue = client.GetCollection<string>("valueTypeNonExist");
            //var genericValue = client.GetCollection<int>("valueTypeNonExist");//compiler error

            // act
            var basicNone = ioBasic.Get<object>(-100);
            var classNone = ioClass.Get(-100);
            var valueNone = ioValue.Get(-100);

            // assert
            Assert.Null(basicNone);
            Assert.Null(classNone);
            Assert.Null(valueNone);
        }
    }
}
