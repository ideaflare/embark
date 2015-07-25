using Embark.Interaction;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestClient.TestData;
using TestClient.TestData.Basic;
using TestClient.TestData.DataEntry;

namespace TestClient
{
    [TestClass]
    public class EdgeCases
    {
        [TestMethod]
        public void GetNonExisting_ReturnsNull()
        {
            // arrange
            var client = Cache.localClient;
            var ioBasic = client["basicNonExist"];
            var ioClass = client.GetCollection<Sheep>("genericClassNonExist");
            var ioValue = client.GetCollection<string>("valueTypeNonExist");
            //var genericValue = client.GetCollection<int>("valueTypeNonExist");//compiler error

            // act
            var basicNone = ioBasic.Get<object>(-100);
            var classNone = ioClass.Get(-100);
            var valueNone = ioValue.Get(-100);

            // assert
            Assert.IsNull(basicNone);
            Assert.IsNull(classNone);
            Assert.IsNull(valueNone);
        }

        [TestMethod]
        public void SaveBlob_CanDeserializeToByteArray()
        {
            // arrange
            byte[] rawByteArray = new byte[64];
            (new Random()).NextBytes(rawByteArray);
            
            var sound = new Sound
            {
                Sample = rawByteArray,
                Echo = null
            };

            long idRawByteArray = Cache.localClient.Basic.Insert(rawByteArray);
            long idSound = Cache.localClient.Basic.Insert(sound);

            // act    
            var loadedSound = Cache.localClient.Basic.Get<Sound>(idSound);
            byte[] loadedSample = loadedSound.Sample;

            byte[] loadedAsArray = Cache.localClient.Basic.Get<byte[]>(idRawByteArray);
            
            // assert
            Assert.IsTrue(Enumerable.SequenceEqual(rawByteArray, loadedAsArray));
            Assert.IsTrue(Enumerable.SequenceEqual(rawByteArray, loadedSample));
        }

        [TestMethod]
        public void SaveNonPoco_HandlesComparison()
        {
            // arrange
            var io = Cache.localClient.GetCollection<string>("nonPOCO");
            string input = "string";
            string inserted;

            // act & assert
            RunAllCommands<string>(io, input, out inserted);
            Assert.AreEqual(input, inserted);            
        }

        [TestMethod]
        public void MixedTypeCollection_CanSave()
        {
            // arrange
            var io = Cache.localClient.GetCollection<object>("MixedDataObjects");
            Sheep inputSheep = new Sheep { Name = "Mittens" };
            object outputObject;
                        
            // act
            io.Insert("non-sheep");
            io.Insert(123);

            var idInput = io.Insert(inputSheep);
            var outputSheep = io.AsBaseCollection().Get<Sheep>(idInput);

            // act & assert
            RunAllCommands(io, inputSheep, out outputObject);

            // TODO move to seperate test
            //var outSheepText = io.TextConverter.ToText(outputObject);
            //Sheep outputSheep = io.TextConverter.ToObject<Sheep>(outSheepText);

            Assert.AreEqual(inputSheep, outputSheep);
        }

        [TestMethod]
        public void GetWhere_MatchesSubProperties()
        {
            // arrange
            var oldWooly = new Sheep { Name = "Wooly", Age = 100, FavouriteIceCream = IceCream.Chocolate };
            var oldDusty = new Sheep { Name = "Dusty", Age = 100, FavouriteIceCream = IceCream.Chocolate, OnTable = new Table { Legs = 2 } };
            var youngLassy = new Sheep { Name = "Lassy", Age = 1, FavouriteIceCream = IceCream.Bubblegum, OnTable = new Table { IsSquare = true } };
            var youngBilly = new Sheep { Name = "Billy", Age = 3, OnTable = new Table { Legs = 2 } };

            var io = Cache.localClient.GetCollection<Sheep>("subMatch");

            long id = io.Insert(oldWooly);
            long id2 = io.Insert(oldDusty);
            long id3 = io.Insert(youngLassy);

            // act            

            IEnumerable<Sheep> matchQueryInline = io.GetWhere(new { Age = 100, OnTable = new { Legs = 2 } }).Unwrap();

            var anonymousTable = new { Legs = 2 };
            var query = new { Age = 100, OnTable = anonymousTable };
            IEnumerable<Sheep> matchQueryAnonymous = io.GetWhere(query).Unwrap();

            var inlineSheep = matchQueryInline.ToList();

            var oldSheepOnTables = matchQueryAnonymous.ToList();

            // assert
            Assert.AreEqual(1, oldSheepOnTables.Count);

            Assert.IsFalse(oldSheepOnTables.Any(s => s.Age != 100));
            Assert.IsFalse(oldSheepOnTables.Any(s => s.OnTable.Legs != 2));

            Assert.IsFalse(oldSheepOnTables.Any(s => s.Name == "Lassy"));
            Assert.IsFalse(oldSheepOnTables.Any(s => s.Name == "Wooly"));
            Assert.IsFalse(oldSheepOnTables.Any(s => s.Name == "Wooly"));
            
            Assert.IsTrue(oldSheepOnTables.Any(s => s.Name == "Dusty"));

            Assert.IsTrue(Enumerable.SequenceEqual(inlineSheep, oldSheepOnTables));
        }

        private static void RunAllCommands<T>(Collection<T> io, T input, out T inserted) where T : class
        {
            // act & assert
            var id = io.Insert(input);

            var all = io.GetAll().ToArray();

            var like = io.GetWhere("string").ToArray();

            var between = io.GetBetween("str", "qqqqing").ToArray();

            inserted = io.Get(id);
        }

        //[TestMethod]
        public void UpdateNonExisting_ReturnsFalse()
        {
            throw new NotImplementedException();
        }
    }
}
