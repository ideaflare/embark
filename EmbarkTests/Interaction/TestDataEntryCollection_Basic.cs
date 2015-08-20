using EmbarkTests._Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmbarkTests.Interaction
{
    [TestClass]
    public class TestDataEntryCollection_Basic
    {
        [TestMethod]
        public void Insert_SetsID()
        {
            // arrange
            var created = Sound.GetTestSound();

            // act
            var saved = MockDB.RuntimeDataEntryCollection.Insert(created);

            // assert
            Assert.AreEqual(created.ID, saved.ID);
            Assert.AreEqual(created.Timestamp, saved.Timestamp);
        }

        [TestMethod]
        public void Get_HasInsertedValues()
        {
            // arrange
            var created = Sound.GetTestSound();
            MockDB.RuntimeDataEntryCollection.Insert(created);

            // act
            var loaded = MockDB.RuntimeDataEntryCollection.Get(created.ID);

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
            MockDB.RuntimeDataEntryCollection.Insert(created);
            var loaded = MockDB.RuntimeDataEntryCollection.Get(created.ID);

            // act
            loaded.Quality = created.Quality + 10;
            var hasUpdated = MockDB.RuntimeDataEntryCollection.Update(loaded);
            var updated = MockDB.RuntimeDataEntryCollection.Get(created.ID);

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
            var inserted = MockDB.RuntimeDataEntryCollection.Insert(created);
            long insertedID = inserted.ID;
            var loaded = MockDB.RuntimeDataEntryCollection.Get(created.ID);

            // act
            bool isDeleted = MockDB.RuntimeDataEntryCollection.Delete(loaded);
            var notFound = MockDB.RuntimeDataEntryCollection.Get(insertedID);
            var nullWrapper = MockDB.RuntimeDataEntryCollection.GetWrapper(insertedID);
            var updateDeleted = MockDB.RuntimeDataEntryCollection.Update(created);

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
            MockDB.RuntimeDataEntryCollection.Insert(registered);
            registered.RegisterAutoUpdate(MockDB.RuntimeDataEntryCollection);

            var nonregistered = MockDB.RuntimeDataEntryCollection.Get(registered.ID);

            // act
            registered.Amplitude = 50;
            int regAmp50 = MockDB.RuntimeDataEntryCollection.Get(nonregistered.ID).Amplitude;
            registered.Amplitude = 12;
            int regAmp12 = MockDB.RuntimeDataEntryCollection.Get(nonregistered.ID).Amplitude;
            registered.Amplitude = 99;

            // assert
            Assert.AreEqual(50, regAmp50);
            Assert.AreEqual(12, regAmp12);
            Assert.AreEqual(99, registered.Amplitude);
            Assert.AreEqual(nonregistered.Amplitude, 100);
        }
    }
}
