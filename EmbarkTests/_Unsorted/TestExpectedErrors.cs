using Microsoft.VisualStudio.TestTools.UnitTesting;
using Embark;
using System;

namespace EmbarkTests._Unsorted
{
    [TestClass]
    public class TestExpectedErrors
    {
        const string testPath = @"C:\MyTemp\Embark\TestErr\";

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Only alphanumerical & underscore characters supported in collection names.")]
        public void CollectionName_OnlyAlphanumericAndUnderScoreSupported()
        {
            var na = Client.GetLocalDB(testPath)["!?$@\filesystem.*"];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Collection name should be at least one alphanumerical or underscore character.")]
        public void CollectionName_CannotBeEmpty()
        {
            var na = Client.GetLocalDB(testPath)[""];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Collection name should be at least one alphanumerical or underscore character.")]
        public void CollectionName_CannotBeNull()
        {
            var na = Client.GetLocalDB(testPath)[null];
        }
    }
}
