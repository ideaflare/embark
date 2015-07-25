using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClient.TestData;

namespace TestClient
{
    [TestClass]
    public class DataEntryBasic
    {
        [TestMethod]
        public void Insert_SetsID()
        {
            // arrange
            var created = TestEntities.GetTestSound();

            // act
            var saved = Cache.DataEntryCollection.Insert(created);

            // assert
            Assert.AreEqual(created.ID, saved.ID);
            Assert.AreEqual(created.Timestamp, saved.Timestamp);
        }
        
        [TestMethod]
        public void Get_HasInsertedValues()
        {
            // arrange
            var created = TestEntities.GetTestSound();
            Cache.DataEntryCollection.Insert(created);

            // act
            var loaded = Cache.DataEntryCollection.Get(created.ID);

            // assert
            Assert.AreEqual(created.ID, loaded.ID);;
            Assert.AreEqual(created.Timestamp, loaded.Timestamp);
            Assert.AreEqual(created.Description, loaded.Description);
            Assert.AreEqual(created.Quality, loaded.Quality);
        }
        
        [TestMethod]
        public void Update_ChangesValues()
        {
            // arrange
            var created = TestEntities.GetTestSound();
            Cache.DataEntryCollection.Insert(created);
            var loaded = Cache.DataEntryCollection.Get(created.ID);

            // act
            loaded.Quality = created.Quality + 10;
            var hasUpdated = Cache.DataEntryCollection.Update(loaded);
            var updated = Cache.DataEntryCollection.Get(created.ID);

            // assert
            Assert.IsTrue(hasUpdated);
            Assert.AreEqual(updated.Quality, loaded.Quality);
            Assert.AreNotEqual(created.Quality, loaded.Quality);
        }

        [TestMethod]
        public void Delete_RemovesEntry()
        {
            // arrange
            var created = TestEntities.GetTestSound();
            long initialID = created.ID;
            var inserted = Cache.DataEntryCollection.Insert(created);
            long insertedID = inserted.ID;
            var loaded = Cache.DataEntryCollection.Get(created.ID);

            // act
            bool isDeleted = Cache.DataEntryCollection.Delete(loaded);
            var notFound = Cache.DataEntryCollection.Get(insertedID);
            var nullWrapper = Cache.DataEntryCollection.GetWrapper(insertedID);
            var updateDeleted = Cache.DataEntryCollection.Update(created);
            
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
            var registered = TestEntities.GetTestSound();
            registered.Amplitude = 100;
            Cache.DataEntryCollection.Insert(registered);
            registered.RegisterAutoUpdate(Cache.DataEntryCollection);

            var nonregistered = Cache.DataEntryCollection.Get(registered.ID);

            // act
            registered.Amplitude = 50;
            int regAmp50 = Cache.DataEntryCollection.Get(nonregistered.ID).Amplitude;
            registered.Amplitude = 12;
            int regAmp12 = Cache.DataEntryCollection.Get(nonregistered.ID).Amplitude;
            registered.Amplitude = 99;

            // assert
            Assert.AreEqual(50, regAmp50);
            Assert.AreEqual(12, regAmp12);
            Assert.AreEqual(99, registered.Amplitude);
            Assert.AreEqual(nonregistered.Amplitude, 100);
        }

    }
}
