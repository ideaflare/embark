using Embark.Interaction;
using EmbarkTests._Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbarkTests.Interaction
{
    [TestClass]
    public class TestDocumentWrapper
    {
        [TestMethod]
        public void GetWrapper_ReturnsDocument()
        {
            // arrange
            var saved = Sheep.GetTestSheep();

            var io = MockDB.SharedRuntimeClient.GetCollection<Sheep>("wrapSelect");

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
            var saved = Sheep.GetTestSheep();
            var io = MockDB.SharedRuntimeClient.GetCollection<Sheep>("wrapUpdate");
            long id = io.Insert(saved);
            DocumentWrapper<Sheep> wrapper = io.GetWrapper(id);

            // act
            var olderAge = saved.Age + 1;
            wrapper.Content.Age = olderAge;
            bool updatedSheep = wrapper.Update();

            var agedSheep = io.Get(id);

            // assert
            Assert.IsTrue(updatedSheep);
            Assert.AreEqual(wrapper.ID, id);
            Assert.AreEqual(olderAge, agedSheep.Age);
        }

        [TestMethod]
        public void WrapperDelete_RemovesDocument()
        {
            // arrange
            var saved = Sheep.GetTestSheep();
            var io = MockDB.SharedRuntimeClient.GetCollection<Sheep>("wrapDelete");
            long id = io.Insert(saved);
            DocumentWrapper<Sheep> wrapper = io.GetWrapper(id);

            // act
            bool deletedDocument = wrapper.Delete();

            bool postDeleteUpdate = wrapper.Update();
            bool postDeleteDelete = wrapper.Delete();

            var missingSheep = io.Get(id);

            // assert
            Assert.IsNull(missingSheep);
            Assert.IsTrue(deletedDocument);
            Assert.IsFalse(postDeleteUpdate);
            Assert.IsFalse(postDeleteDelete);
        }

        [TestMethod]
        public void WrapperToString_EqualsUnwrappedToString()
        {
            // arrange
            var sheep = Sheep.GetTestSheep();
            var io = MockDB.SharedRuntimeClient.GetCollection<Sheep>("WrapperToString");

            // act
            var id = io.Insert(sheep);
            var wrappedSheep = io.GetAll().Single();

            // assert
            Assert.AreEqual(sheep.ToString(), wrappedSheep.ToString());
            Assert.AreEqual(wrappedSheep.ToString(), wrappedSheep.Content.ToString());
        }

        [TestMethod]
        public void GetNonExistingWrapper_ReturnsNull()
            => Assert.IsNull(MockDB.RuntimeBasicCollection.GetWrapper<Sheep>(-1));
    }
}
