using Embark.Interaction.MVVM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestClient.MVVM
{
    //[TestClass]
    public partial class MVVM
    {
        [TestMethod]
        public void PropertyChangeBase_RaisesEventsAsExpected()
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

            bool dumbellsWasObserved = false;

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
            dumbellsWasObserved = lifter.RaisePropertyChangedEvent((raiseThe) => raiseThe.Dumbbells);

            //Assert
            Assert.IsTrue(nameChanged);
            Assert.IsTrue(nameChanging);
            Assert.AreEqual("Alex", lifter.NickName);

            Assert.IsFalse(heightChanged);
            Assert.IsFalse(heightChanging);
            Assert.AreEqual(6.2, lifter.Height);

            Assert.IsTrue(dumbbellsraised);
            Assert.AreEqual(2, lifter.Dumbbells);

            Assert.IsTrue(dumbellsWasObserved);
        }

        [TestMethod]
        public void GetPropertyString_ReturnsSameName()
        {
            var result = (new WeightLifter()).GetPropertyString((vm) => vm.Dumbbells);
            Assert.AreEqual("Dumbbells", result);
        }
    }

    public class WeightLifter : PropertyChangeBase
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
