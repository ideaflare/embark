using Embark.Interaction;
using EmbarkTests._Mocks;
using Xunit;
using System.Linq;

namespace EmbarkTests.Interaction
{
    public class DocumentWrapperTests
    {
        [Fact]
        public void GetWrapper_ReturnsDocument()
        {
            // arrange
            var saved = Sheep.GetTestSheep();

            var io = _MockDB.SharedRuntimeClient.GetCollection<Sheep>("wrapSelect");

            long id = io.Insert(saved);

            // act
            DocumentWrapper<Sheep> wrapper = io.GetWrapper(id);

            var loaded = wrapper.Content;

            // assert
            Assert.Equal(wrapper.ID, id);
            Assert.Equal(saved.Name, loaded.Name);
            Assert.Equal(saved.Age, loaded.Age);
            Assert.Equal(saved.FavouriteIceCream, loaded.FavouriteIceCream);
        }

        [Fact]
        public void WrapperUpdate_CommitsChanges()
        {
            // arrange
            var saved = Sheep.GetTestSheep();
            var io = _MockDB.SharedRuntimeClient.GetCollection<Sheep>("wrapUpdate");
            long id = io.Insert(saved);
            DocumentWrapper<Sheep> wrapper = io.GetWrapper(id);

            // act
            var olderAge = saved.Age + 1;
            wrapper.Content.Age = olderAge;
            bool updatedSheep = wrapper.Update();

            var agedSheep = io.Get(id);

            // assert
            Assert.True(updatedSheep);
            Assert.Equal(wrapper.ID, id);
            Assert.Equal(olderAge, agedSheep.Age);
        }

        [Fact]
        public void WrapperDelete_RemovesDocument()
        {
            // arrange
            var saved = Sheep.GetTestSheep();
            var io = _MockDB.SharedRuntimeClient.GetCollection<Sheep>("wrapDelete");
            long id = io.Insert(saved);
            DocumentWrapper<Sheep> wrapper = io.GetWrapper(id);

            // act
            bool deletedDocument = wrapper.Delete();

            bool postDeleteUpdate = wrapper.Update();
            bool postDeleteDelete = wrapper.Delete();

            var missingSheep = io.Get(id);

            // assert
            Assert.Null(missingSheep);
            Assert.True(deletedDocument);
            Assert.False(postDeleteUpdate);
            Assert.False(postDeleteDelete);
        }

        [Fact]
        public void WrapperToString_EqualsUnwrappedToString()
        {
            // arrange
            var sheep = Sheep.GetTestSheep();
            var io = _MockDB.SharedRuntimeClient.GetCollection<Sheep>("WrapperToString");

            // act
            var id = io.Insert(sheep);
            var wrappedSheep = io.GetAll().Single();

            // assert
            Assert.Equal(sheep.ToString(), wrappedSheep.ToString());
            Assert.Equal(wrappedSheep.ToString(), wrappedSheep.Content.ToString());
        }

        [Fact]
        public void GetNonExistingWrapper_ReturnsNull()
            => Assert.Null(_MockDB.RuntimeBasicCollection.GetWrapper<Sheep>(-1));
    }
}
