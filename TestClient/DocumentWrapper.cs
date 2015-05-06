using Embark.TextConversion;
using Embark.Interaction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.TestData;
using TestClient.TestData.Basic;

namespace TestClient
{
    [TestClass]
    public class DocumentWrapper
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

            var loaded = wrapper.Content;

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
            wrapper.Content.Age = olderAge;
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

        [TestMethod]
        public void WrapperToString_EqualsUnwrappedToString()
        {
            // arrange
            var sheep = TestEntities.GetTestSheep();
            var io = Cache.localClient.GetCollection<Sheep>("WrapperToString");

            // act
            var id = io.Insert(sheep);
            var wrappedSheep = io.GetAll().Single();

            // assert
            Assert.AreEqual(sheep.ToString(), wrappedSheep.ToString());
            Assert.AreEqual(wrappedSheep.ToString(), wrappedSheep.Content.ToString());
        }



        [TestMethod]
        public void GetNonExistingWrapper_ReturnsNull()
        {
            Assert.IsNull(Cache.BasicCollection.GetWrapper<Sheep>(-1));
        }

    }
}
