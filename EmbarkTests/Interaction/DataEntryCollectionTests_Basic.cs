using EmbarkTests._Mocks;
using Xunit;

namespace EmbarkTests.Interaction
{
    public class DataEntryCollectionTests_Basic
    {
        [Fact]
        public void Insert_SetsID()
        {
            // arrange
            var created = Sound.GetTestSound();

            // act
            var saved = _MockDB.RuntimeDataEntryCollection.Insert(created);

            // assert
            Assert.Equal(created.ID, saved.ID);
            Assert.Equal(created.Timestamp, saved.Timestamp);
        }

        [Fact]
        public void Get_HasInsertedValues()
        {
            // arrange
            var created = Sound.GetTestSound();
            _MockDB.RuntimeDataEntryCollection.Insert(created);

            // act
            var loaded = _MockDB.RuntimeDataEntryCollection.Get(created.ID);

            // assert
            Assert.Equal(created.ID, loaded.ID); ;
            Assert.Equal(created.Timestamp, loaded.Timestamp);
            Assert.Equal(created.Description, loaded.Description);
            Assert.Equal(created.Quality, loaded.Quality);
        }

        [Fact]
        public void Update_ChangesValues()
        {
            // arrange
            var created = Sound.GetTestSound();
            _MockDB.RuntimeDataEntryCollection.Insert(created);
            var loaded = _MockDB.RuntimeDataEntryCollection.Get(created.ID);

            // act
            loaded.Quality = created.Quality + 10;
            var hasUpdated = _MockDB.RuntimeDataEntryCollection.Update(loaded);
            var updated = _MockDB.RuntimeDataEntryCollection.Get(created.ID);

            // assert
            Assert.True(hasUpdated);
            Assert.Equal(updated.Quality, loaded.Quality);
            Assert.NotEqual(created.Quality, loaded.Quality);
        }

        [Fact]
        public void Delete_RemovesEntry()
        {
            // arrange
            var created = Sound.GetTestSound();
            long initialID = created.ID;
            var inserted = _MockDB.RuntimeDataEntryCollection.Insert(created);
            long insertedID = inserted.ID;
            var loaded = _MockDB.RuntimeDataEntryCollection.Get(created.ID);

            // act
            bool isDeleted = _MockDB.RuntimeDataEntryCollection.Delete(loaded);
            var notFound = _MockDB.RuntimeDataEntryCollection.Get(insertedID);
            var nullWrapper = _MockDB.RuntimeDataEntryCollection.GetWrapper(insertedID);
            var updateDeleted = _MockDB.RuntimeDataEntryCollection.Update(created);

            // assert
            Assert.True(isDeleted);
            Assert.Null(notFound);
            Assert.Null(nullWrapper);
            Assert.NotEqual(initialID, insertedID);
            Assert.Equal(created.ID, inserted.ID);
            Assert.False(updateDeleted);
        }

        [Fact]
        public void AutoUpdate_ChangesValues()
        {
            // arrange
            var registered = Sound.GetTestSound();
            registered.Amplitude = 100;
            _MockDB.RuntimeDataEntryCollection.Insert(registered);
            registered.RegisterAutoUpdate(_MockDB.RuntimeDataEntryCollection);

            var nonregistered = _MockDB.RuntimeDataEntryCollection.Get(registered.ID);

            // act
            registered.Amplitude = 50;
            int regAmp50 = _MockDB.RuntimeDataEntryCollection.Get(nonregistered.ID).Amplitude;
            registered.Amplitude = 12;
            int regAmp12 = _MockDB.RuntimeDataEntryCollection.Get(nonregistered.ID).Amplitude;
            registered.Amplitude = 99;

            // assert
            Assert.Equal(50, regAmp50);
            Assert.Equal(12, regAmp12);
            Assert.Equal(99, registered.Amplitude);
            Assert.Equal(nonregistered.Amplitude, 100);
        }
    }
}
