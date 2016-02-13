using Xunit;
using Embark;
using System;

namespace EmbarkTests._Unsorted
{
    public class TestExpectedErrors
    {
        string testPath = @"C:\MyTemp\Embark\TestErr\";
        
        [Fact]
        public void CollectionName_OnlyAlphanumericAndUnderScoreSupported()
        {
            string invalidCharacterMessage = "Only alphanumerical & underscore characters supported in collection names.";

            var err = Assert.Throws<NotSupportedException>(() =>
            {
                var na = Client.GetLocalDB(testPath)["!?$@\filesystem.*"];
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
                var na = Client.GetLocalDB(testPath)[collectionName];
            });

            Assert.Equal(invalidLengthMessage, err.Message);
        }
    }
}