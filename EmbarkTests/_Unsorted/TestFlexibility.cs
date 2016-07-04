using EmbarkTests._Mocks;
using Xunit;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EmbarkTests._Unsorted
{
    public class TestFlexibility
    {    
        [Fact]
        public void TypofString_CanDeserialize()
        {
            // arrange
            var io = _MockDB.SharedRuntimeClient["stringTest"];

            var savedText = "Save just a string to DB, not a class with public properties";

            long idText = io.Insert(savedText);            
            
            // act
            string loadedText = io.Get<string>(idText);

            // assert
            Assert.Equal(savedText, loadedText);
        }
        
        [Fact]
        public void ValueTypes_CanTurnIntoText()
        {
            // arrange
            var io = _MockDB.SharedRuntimeClient["valuetypeToString"];

            int savedInt = 561;
            long savedLong = long.MaxValue * -1;
            double savedDouble = double.MaxValue;
            bool savedBool = true;
            char savedChar = '\x0058';

            long idInt = io.Insert((object)savedInt);
            long idLong = io.Insert((object)savedLong);
            long idDouble = io.Insert((object)savedDouble);
            long idBool = io.Insert((object)savedBool);
            long idChar = io.Insert((object)savedChar);

            string loadedInt = io.Get<string>(idInt);
            string loadedLong = io.Get<string>(idLong);
            string loadedDouble = io.Get<string>(idDouble);
            string loadedBool = io.Get<string>(idBool);
            string loadedChar = io.Get<string>(idChar);

            Assert.Equal(savedInt.ToString(), loadedInt);
            Assert.Equal(savedLong.ToString(), loadedLong);            
            Assert.Equal(savedBool.ToString(), loadedBool);
            Assert.Equal(savedChar.ToString(), loadedChar);

            // double type tries to add more precision, ignore with "R".
            // deserializer/locality different rules on , or . separator
            var saveDoubleToString = savedDouble.ToString("R").Replace(",", ".");
            var loadedDoubleToString = loadedDouble.Replace(",", ".");
            Assert.Equal(saveDoubleToString, loadedDoubleToString);
        }

        [Fact]
        public void ArrayObjects_SerializesAsString()
        {
            var io = _MockDB.SharedRuntimeClient["numberArrayToString"];

            byte[] arr = new byte[] { 12, 200, 12, 0, 33 };

            long idByteArr = io.Insert(arr);

            string loadedByteArr = io.Get<string>(idByteArr);

            var match = Regex.Match(loadedByteArr, @"(12\D*200\D*12\D*0\D*33)");

            Assert.True(match.Success);
        }

        [Fact]
        public void ComposedObject_SerializesAsString()
        { 
            var io = _MockDB.SharedRuntimeClient["composedObjectToString"];
            
            var composedObject = new List<object>() 
            {
                "32", 'x', new int[] { 4, 4 }, 2,
                new Sound 
                {
                    Description = "Multi type Test",
                    Echo = new Echo { Repetitions = 6}
                }
            };
            long idList = io.Insert(composedObject);

            var textPattern = @".*32.*x.*4.*2"
                + ".*Sound.*Description.*Multi.*Echo.*Repititions.*6"
                + ".*Sample.*Timestamp"; //other properties in class Sound

            // act            
            string textObject = io.Get<string>(idList);
            var objectList = io.Get<List<object>>(idList);

            // assert            
            var match = Regex.Match(textObject, textPattern, RegexOptions.Singleline);
            Assert.Equal(objectList.Count, composedObject.Count);
        }

        // TODO 1 Test Save/load different types - primitive, POCO & IDataEntry serialize/deserialize behaviour

        // insert
        //long id = io.Insert(pet);
        //long d2 = io.Insert((object)500);
        //long d3 = io.Insert("a string!");
        //long d4 = io.Insert<Cat>(mittens);

        //// get
        //Sheep fluffy = io.Get<Sheep>(id);

        //var xs = (int)io.Get<object>(d2);
        //var ss = io.Get<string>(d4); err
        //Cat indy = io.Get<Cat>(d2); err
        //var x = io.Get<Object>(d3); 
        //string empty = io.Get<string>(d4); err        
    }
}
