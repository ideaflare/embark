using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClient.IO;
using System.Linq;
using System.Diagnostics;
using TestClient.TestData;
using TestClient.IO.TestData;

namespace TestClient
{
    [TestClass]
    public class Functionality
    {
        //operational
        [TestMethod]
        public void NewIDs_AreUnique()
        {
            int totalInserts = 16;
            double timePerInsert = 7;// milliseconds per insert. 
            // Test written on laptop with Samsung 840 SSD. Increase insert time if machine uses a spinning magnetic relic.

            // insert IDs in parallel
            var sw = Stopwatch.StartNew();
            var newIDs = Enumerable.Range(0, totalInserts)
                .AsParallel()
                .Select(i => Cache.BasicCollection.Insert(new { n = 0 }))
                .ToList();
            sw.Stop();

            // test that they are unique
            Assert.AreEqual(totalInserts, newIDs.Distinct().Count());
            // and completed within average timePerInsert time
            Assert.IsTrue(sw.ElapsedMilliseconds < timePerInsert * totalInserts);
        }

        [TestMethod]
        public void CreatedID_IsTimeStamp()
        {
            // arrange
            var now = DateTime.Now;
            long id = Cache.BasicCollection.Insert(new { Numero = "Uno" });

            // act
            var timestamp = new DateTime(id);

            // assert
            var timeDiff = timestamp.Subtract(now);
            Assert.IsTrue(timeDiff.TotalSeconds < 1);
        }

        [TestMethod]
        public void Sheep_CanTurnIntoACat()
        {
            var sheep = TestEntities.GetTestSheep();

            long id = Cache.BasicCollection.Insert(sheep);

            Cat cat = Cache.BasicCollection.Get<Cat>(id);

            Assert.AreEqual(cat.Name, sheep.Name);
            Assert.AreEqual(cat.Age, sheep.Age);
            Assert.AreEqual(cat.FurDensity, (new Cat()).FurDensity);
            Assert.AreEqual(cat.HasMeme, (new Cat()).HasMeme);
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

        //[TestMethod]
        public void TestObject_IsRevarsableSerializable()
        {
            throw new NotImplementedException();
        }
    }
}
