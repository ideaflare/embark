using Xunit;
using Embark.Interaction;
using System;
using System.Linq;
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
