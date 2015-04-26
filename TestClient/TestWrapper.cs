using Embark.Conversion;
using Embark.Interaction;
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
            var saved = TestEntities.GetTestSheep();

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
            var saved = TestEntities.GetTestSheep();
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

        [TestMethod]
        public void WrapperDelete_RemovesDocument()
        {
            // arrange
            var saved = TestEntities.GetTestSheep();
            var io = Cache.localClient.GetCollection<Sheep>("wrapDelete");
            long id = io.Insert(saved);
            DocumentWrapper<Sheep> wrapper = io.GetWrapper(id);

            // act
            wrapper.Delete();

            bool postDeleteUpdate = wrapper.Update();

            var missingSheep = io.Get(id);

            // assert
            Assert.IsNull(missingSheep);
            Assert.IsFalse(postDeleteUpdate);
        }

    }
}
