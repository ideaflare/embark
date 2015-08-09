using EmbarkTests._Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmbarkTests.Interaction
{
    [TestClass]
    public class DataEntryCollectionTestsBasic
    {
        [TestMethod]
        public void Insert_SetsID()
        {
            // arrange
            var created = Sound.GetTestSound();

            // act
            var saved = MockDB.DataEntryCollection.Insert(created);

            // assert
            Assert.AreEqual(created.ID, saved.ID);
            Assert.AreEqual(created.Timestamp, saved.Timestamp);
        }

        [TestMethod]
        public void Get_HasInsertedValues()
        {
            // arrange
            var created = Sound.GetTestSound();
            MockDB.DataEntryCollection.Insert(created);

            // act
            var loaded = MockDB.DataEntryCollection.Get(created.ID);

            // assert
            Assert.AreEqual(created.ID, loaded.ID); ;
            Assert.AreEqual(created.Timestamp, loaded.Timestamp);
            Assert.AreEqual(created.Description, loaded.Description);
            Assert.AreEqual(created.Quality, loaded.Quality);
        }

        [TestMethod]
        public void Update_ChangesValues()
        {
            // arrange
            var created = Sound.GetTestSound();
            MockDB.DataEntryCollection.Insert(created);
            var loaded = MockDB.DataEntryCollection.Get(created.ID);

            // act
            loaded.Quality = created.Quality + 10;
            var hasUpdated = MockDB.DataEntryCollection.Update(loaded);
            var updated = MockDB.DataEntryCollection.Get(created.ID);

            // assert
            Assert.IsTrue(hasUpdated);
            Assert.AreEqual(updated.Quality, loaded.Quality);
            Assert.AreNotEqual(created.Quality, loaded.Quality);
        }

        [TestMethod]
        public void Delete_RemovesEntry()
        {
            // arrange
            var created = Sound.GetTestSound();
            long initialID = created.ID;
            var inserted = MockDB.DataEntryCollection.Insert(created);
            long insertedID = inserted.ID;
            var loaded = MockDB.DataEntryCollection.Get(created.ID);

            // act
            bool isDeleted = MockDB.DataEntryCollection.Delete(loaded);
            var notFound = MockDB.DataEntryCollection.Get(insertedID);
            var nullWrapper = MockDB.DataEntryCollection.GetWrapper(insertedID);
            var updateDeleted = MockDB.DataEntryCollection.Update(created);

            // assert
            Assert.IsTrue(isDeleted);
            Assert.IsNull(notFound);
            Assert.IsNull(nullWrapper);
            Assert.AreNotEqual(initialID, insertedID);
            Assert.AreEqual(created.ID, inserted.ID);
            Assert.IsFalse(updateDeleted);
        }

        [TestMethod]
        public void AutoUpdate_ChangesValues()
        {
            // arrange
            var registered = Sound.GetTestSound();
            registered.Amplitude = 100;
            MockDB.DataEntryCollection.Insert(registered);
            registered.RegisterAutoUpdate(MockDB.DataEntryCollection);

            var nonregistered = MockDB.DataEntryCollection.Get(registered.ID);

            // act
            registered.Amplitude = 50;
            int regAmp50 = MockDB.DataEntryCollection.Get(nonregistered.ID).Amplitude;
            registered.Amplitude = 12;
            int regAmp12 = MockDB.DataEntryCollection.Get(nonregistered.ID).Amplitude;
            registered.Amplitude = 99;

            // assert
            Assert.AreEqual(50, regAmp50);
            Assert.AreEqual(12, regAmp12);
            Assert.AreEqual(99, registered.Amplitude);
            Assert.AreEqual(nonregistered.Amplitude, 100);
        }
    }
}
