using Embark.Conversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.IO.TestData;
using TestClient.TestData;

namespace TestClient
{
    [TestClass]
    public class TestWrapper
    {
        [TestMethod]
        public void GetWrapper_ReturnsDocument()
        {
            // arrange
            var saved = Animals.GetTestSheep();

            var io = Cache.localClient.GetCollection<Sheep>("wrapSelect");

            long id = io.Insert(saved);

            // act
            DocumentWrapper<Sheep> wrapper = io.GetWrapper(id);

            var loaded = wrapper.Value;

            // assert
            Assert.AreEqual(wrapper.ID, id);
            Assert.AreEqual(saved.Name, loaded.Name);
            Assert.AreEqual(saved.Age, loaded.Age);
            Assert.AreEqual(saved.FavouriteIceCream, loaded.FavouriteIceCream);   
        }

        
        [TestMethod]
        public void WrapperUpdate_CommitsChanges()
        {
            // arrange
            var saved = Animals.GetTestSheep();
            var io = Cache.localClient.GetCollection<Sheep>("wrapUpdate");
            long id = io.Insert(saved);
            DocumentWrapper<Sheep> wrapper = io.GetWrapper(id);

            // act
            var olderAge = saved.Age + 1;
            wrapper.Value.Age = olderAge;
            wrapper.Update();

            var agedSheep = io.Get(id);

            // assert
            Assert.AreEqual(wrapper.ID, id);

            Assert.AreEqual(olderAge, agedSheep.Age);
        }
    }
}
