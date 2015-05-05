using Embark;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    [TestClass]
    public class ExpectedErrors
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Only alphanumerical & underscore characters supported in collection names.")]
        public void CollectionName_OnlyAlphanumericAndUnderScoreSupported()
        {
            var na = Client.GetLocalDB()["!?$@\filesystem.*"];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Collection name should be at least one alphanumerical or underscore character.")]
        public void CollectionName_CannotBeEmpty()
        {
            var na = Client.GetLocalDB()[""];
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Collection name should be at least one alphanumerical or underscore character.")]
        public void CollectionName_CannotBeNull()
        {
            var na = Client.GetLocalDB()[null];
        }
    }
}
