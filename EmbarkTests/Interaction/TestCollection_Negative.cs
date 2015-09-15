using EmbarkTests._Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmbarkTests.Interaction
{
    [TestClass]
    public class TestCollection_Negative
    {
        [TestMethod]
        public void GetNonExisting_ReturnsNull()
        {
            // arrange
            var client = MockDB.SharedRuntimeClient;
            var ioBasic = client["basicNonExist"];
            var ioClass = client.GetCollection<Sheep>("genericClassNonExist");
            var ioValue = client.GetCollection<string>("valueTypeNonExist");
            //var genericValue = client.GetCollection<int>("valueTypeNonExist");//compiler error

            // act
            var basicNone = ioBasic.Get<object>(-100);
            var classNone = ioClass.Get(-100);
            var valueNone = ioValue.Get(-100);

            // assert
            Assert.IsNull(basicNone);
            Assert.IsNull(classNone);
            Assert.IsNull(valueNone);
        }
    }
}
