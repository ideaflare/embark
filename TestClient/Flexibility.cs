using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestClient.TestData;
using TestClient.TestData.Basic;

namespace TestClient
{
    [TestClass]
    public class Flexibility
    {
        [TestMethod]
        public void Sheep_CanTurnIntoCat()
        {
            // arrange
            var sheep = TestEntities.GetTestSheep();

            long id = Cache.BasicCollection.Insert(sheep);

            // act
            Cat cat = Cache.BasicCollection.Get<Cat>(id);

            // assert
            Assert.AreEqual(cat.Name, sheep.Name);
            Assert.AreEqual(cat.Age, sheep.Age);
            Assert.AreEqual(cat.FurDensity, (new Cat()).FurDensity);
            Assert.AreEqual(cat.HasMeme, (new Cat()).HasMeme);
        }
        
        [TestMethod]
        public void TypofString_CanDeserialize()
        {
            // arrange
            var io = Cache.localClient["stringTest"];

            var savedText = "Save just a string to DB, not a class with public properties";

            long idText = io.Insert(savedText);            
            
            // act
            string loadedText = io.Get<string>(idText);

            // assert
            Assert.AreEqual(savedText, loadedText);
        }
        
        [TestMethod]
        public void ValueTypes_CanTurnIntoText()
        {
            // arrange
            var io = Cache.localClient["valuetypeToString"];

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

            Assert.AreEqual(savedInt.ToString(), loadedInt);
            Assert.AreEqual(savedLong.ToString(), loadedLong);            
            Assert.AreEqual(savedBool.ToString(), loadedBool);
            Assert.AreEqual(savedChar.ToString(), loadedChar);

            // double type tries to add more precision, ignore with "R".
            // deserializer/locality different rules on , or . separator
            var saveDoubleToString = savedDouble.ToString("R").Replace(",", ".");
            var loadedDoubleToString = loadedDouble.Replace(",", ".");
            Assert.AreEqual(saveDoubleToString, loadedDoubleToString);
        }

        [TestMethod]
        public void Cat_CanTurnIntoJsonText()
        {
            // arrange
            var io = Cache.localClient["classToJsonText"];
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
            Assert.AreEqual(jsonCat, objectAsString);
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
