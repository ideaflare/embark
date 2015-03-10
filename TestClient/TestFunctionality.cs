using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClient.IO;
using System.Linq;
using System.Diagnostics;

namespace TestClient
{
    [TestClass]
    public class TestFunctionality
    {
        //operational
        [TestMethod]
        public void NewIDs_AreUnique()
        {
            int totalInserts = 200;
            double timePerInsert = 7;// milliseconds per insert

            // insert IDs in parallel
            var sw = Stopwatch.StartNew();
            var newIDs = Enumerable.Range(0, totalInserts)
                .AsParallel()
                .Select(i => TestData.localSheep.Insert(new { Number = i, Text = "Hi" }))
                .ToList();
            sw.Stop();

            // test that they are unique
            Assert.AreEqual(newIDs.Count, newIDs.Distinct().Count());
            // and completed under a second
            Assert.IsTrue(sw.ElapsedMilliseconds < timePerInsert * totalInserts);

            // cleanup
            newIDs.AsParallel().ForAll(id => TestData.localSheep.Delete(id));
        }

        [TestMethod]
        public void CreatedID_IsTimeStamp()
        {
            // arrange
            var now = DateTime.Now;
            long id = TestData.localSheep.Insert(new { Numero = "Uno" });

            // act
            var timestamp = new DateTime(id);

            // assert
            var timeDiff = timestamp.Subtract(now);
            Assert.IsTrue(timeDiff.TotalSeconds < 1);

            // cleanup
            TestData.localSheep.Delete(id);
        }

        [TestMethod]
        public void Sheep_CanTurnIntoACat()
        {
            var sheep = Animals.GetTestSheep();

            long id = TestData.localSheep.Insert(sheep);

            Cat cat = TestData.localSheep.Select<Cat>(id);

            Assert.AreEqual(cat.Name, sheep.Name);
            Assert.AreEqual(cat.Age, sheep.Age);
            Assert.AreEqual(cat.FurDensity, (new Cat()).FurDensity);
            Assert.AreEqual(cat.HasMeme, (new Cat()).HasMeme);

            // cleanup
            TestData.localSheep.Delete(id);
        }

        //[TestMethod]
        public void TestObject_IsRevarsableSerializable()
        {
            throw new NotImplementedException();
        }
    }
}
