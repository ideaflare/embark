using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClient.TestData;
using TestClient.IO.TestData;

namespace TestClient
{
    [TestClass]
    public class Flexibility
    {
        // TODO 1 Test Save/load different types - primitive, POCO & IDataEntry serialize/deserialize behaviour

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
    }
}
