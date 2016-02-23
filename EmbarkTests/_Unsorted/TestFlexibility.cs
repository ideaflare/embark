using EmbarkTests._Mocks;
using Xunit;
using System.Collections.Generic;

namespace EmbarkTests._Unsorted
{
    public class TestFlexibility
    {
        [Fact]
        public void Sheep_CanTurnIntoCat()
        {
            // arrange
            var sheep = Sheep.GetTestSheep();

            long id = _MockDB.RuntimeBasicCollection.Insert(sheep);

            // act
            Cat cat = _MockDB.RuntimeBasicCollection.Get<Cat>(id);

            // assert
            Assert.Equal(cat.Name, sheep.Name);
            Assert.Equal(cat.Age, sheep.Age);
            Assert.Equal(cat.FurDensity, (new Cat()).FurDensity);
            Assert.Equal(cat.HasMeme, (new Cat()).HasMeme);
        }
        
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
        public void ArrayObjects_CanTurnIntoText()
        {
            // arrange
            var io = _MockDB.SharedRuntimeClient["arraysToString"];

            byte[] arr = new byte[] { 12, 200, 12, 0, 33 };
            var byteArrString = "[\r\n   12,\r\n   200,\r\n   12,\r\n   0,\r\n   33\r\n]";
            long idByteArr = io.Insert(arr);

            var mixedList = new List<object>() 
            {
                "32", 'x', new int[] { 4, 4 }, 2,
                new Sound 
                {
                    Description = "Multi type Test",
                    Echo = new Echo { Repetitions = 6}
                }
            };
            long idList = io.Insert(mixedList);
            var listString = "[\r\n   \"32\",\r\n   \"x\",\r\n   [\r\n      4,\r\n      4\r\n   ],\r\n   2,\r\n   {\r\n      "
            + "\"Description\" : \"Multi type Test\",\r\n      \"Quality\" : 0,\r\n      \"Amplitude\" : 0,\r\n      \"Echo\" : {\r\n         \"Repetitions\" : 6,\r\n"
            + "         \"VolumeDiminishFactor\" : 0\r\n      },\r\n      \"Sample\" : null,\r\n      \"ID\" : 0,\r\n"
            + "      \"Timestamp\" : \"\\/Date(-62135596800000)\\/\"\r\n   }\r\n]";

            // act
            string loadedByteArr = io.Get<string>(idByteArr);
            string loadedList = io.Get<string>(idList);

            var objectList = io.Get<List<object>>(idList);

            // assert
            Assert.Equal(byteArrString, loadedByteArr);
            Assert.Equal(listString, loadedList);

            Assert.NotNull(objectList);
        }

        [Fact]
        public void Cat_CanTurnIntoJsonText()
        {
            // arrange
            var io = _MockDB.SharedRuntimeClient["classToJsonText"];
            var savedCat = new Cat
            {
                Name = "Tom",
                Tale = @"Deserializer can mistake as text object instead of vanilla string."
            };
            var jsonCat = "{\r\n   \"Name\" : \"Tom\",\r\n   \"Age\" : 0,\r\n"
                +"   \"Tale\" : \"Deserializer can mistake as text object instead of vanilla string.\",\r\n"
                +"   \"FurDensity\" : 0,\r\n   \"HasMeme\" : false\r\n}";

            var idCat = io.Insert(savedCat);

            // act
            string objectAsString = io.Get<string>(idCat);

            // assert}
            Assert.Equal(jsonCat, objectAsString);
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
