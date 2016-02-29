using Xunit;
using Embark.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmbarkTests._Mocks;

namespace EmbarkTests._Unsorted
{
    public class TestEdgeCases
    {
        [Fact]
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

            long idRawByteArray = _MockDB.SharedRuntimeClient.Basic.Insert(rawByteArray);
            long idSound = _MockDB.SharedRuntimeClient.Basic.Insert(sound);

            // act    
            var loadedSound = _MockDB.SharedRuntimeClient.Basic.Get<Sound>(idSound);
            byte[] loadedSample = loadedSound.Sample;

            byte[] loadedAsArray = _MockDB.SharedRuntimeClient.Basic.Get<byte[]>(idRawByteArray);
            
            // assert
            Assert.True(Enumerable.SequenceEqual(rawByteArray, loadedAsArray));
            Assert.True(Enumerable.SequenceEqual(rawByteArray, loadedSample));
        }

        [Fact]
        public void MixedTypeCollection_CanSave()
        {
            // arrange
            var io = _MockDB.SharedRuntimeClient.GetCollection<object>("MixedDataObjects");
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

            Assert.Equal(inputSheep, outputSheep);
        }

        [Fact]
        public void GetWhere_MatchesSubProperties()
        {
            // arrange
            var oldWooly = new Sheep { Name = "Wooly", Age = 100, FavouriteIceCream = IceCream.Chocolate };
            var oldDusty = new Sheep { Name = "Dusty", Age = 100, FavouriteIceCream = IceCream.Chocolate, OnTable = new Table { Legs = 2 } };
            var youngLassy = new Sheep { Name = "Lassy", Age = 1, FavouriteIceCream = IceCream.Bubblegum, OnTable = new Table { IsSquare = true } };
            var youngBilly = new Sheep { Name = "Billy", Age = 3, OnTable = new Table { Legs = 2 } };

            var io = _MockDB.SharedRuntimeClient.GetCollection<Sheep>("subMatch");

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
            Assert.Equal(1, oldSheepOnTables.Count);

            Assert.False(oldSheepOnTables.Any(s => s.Age != 100));
            Assert.False(oldSheepOnTables.Any(s => s.OnTable.Legs != 2));

            Assert.False(oldSheepOnTables.Any(s => s.Name == "Lassy"));
            Assert.False(oldSheepOnTables.Any(s => s.Name == "Wooly"));
            Assert.False(oldSheepOnTables.Any(s => s.Name == "Wooly"));
            
            Assert.True(oldSheepOnTables.Any(s => s.Name == "Dusty"));

            Assert.True(Enumerable.SequenceEqual(inlineSheep, oldSheepOnTables));
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

        
    }
}
