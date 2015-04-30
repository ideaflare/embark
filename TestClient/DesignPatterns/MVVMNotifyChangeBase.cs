using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Embark.DesignPatterns.MVVM;

namespace TestClient.DesignPatterns
{
    //[TestClass]
    public partial class MVVM
    {

        [TestMethod]
        public void SetProperty_RaisesEventsAsExpected()
        {
            //Arrange
            var lifter = new WeightLifter
            {
                NickName = "Andre",
                Dumbbells = 2,
                Height = 6.2
            };

            bool dumbbellsraised = false;
            bool nameChanged = false;
            bool nameChanging = false;
            bool heightChanged = false;
            bool heightChanging = false;

            lifter.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "NickName")
                        nameChanged = true;
                    if (e.PropertyName == "Dumbbells")
                        dumbbellsraised = true;
                    if (e.PropertyName == "Height")
                        heightChanged = true;
                };
            lifter.PropertyChanging += (s, e) =>
            {
                if (e.PropertyName == "NickName")
                    nameChanging = true;
                if (e.PropertyName == "Height")
                    heightChanging = true;
            };

            //Act
            lifter.NickName = "Alex";
            lifter.Height = 6.2;
            lifter.RaisePropertyChangedEvent((raiseThe) => raiseThe.Dumbbells);

            //Assert
            Assert.IsTrue(nameChanged);
            Assert.IsTrue(nameChanging);
            Assert.AreEqual("Alex", lifter.NickName);

            Assert.IsFalse(heightChanged);
            Assert.IsFalse(heightChanging);
            Assert.AreEqual(6.2, lifter.Height);

            Assert.IsTrue(dumbbellsraised);
            Assert.AreEqual(2, lifter.Dumbbells);
        }
    }

    public class WeightLifter : NotifyChangeBase
    {
        public int Dumbbells { get; set; }

        private string nickName = "";
        public string NickName
        {
            get { return this.nickName; }
            set { SetProperty(ref this.nickName, value); }
        }

        private double height;
        public double Height
        {
            get { return this.height; }
            set { SetProperty(ref this.height, value); }
        }
    }
}
