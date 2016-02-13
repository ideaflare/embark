using EmbarkTests._Mocks;
using Xunit;

namespace EmbarkTests.Interaction
{
    public class TestCollection_Negative
    {
        [Fact]
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
            Assert.Null(basicNone);
            Assert.Null(classNone);
            Assert.Null(valueNone);
        }
    }
}
