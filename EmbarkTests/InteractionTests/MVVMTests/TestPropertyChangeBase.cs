using Microsoft.VisualStudio.TestTools.UnitTesting;
using Embark.Interaction.MVVM;
using EmbarkTests.InteractionTests.MVVMTests;

namespace TestClient.MVVM
{
    [TestClass]
    public class TestPropertyChangeBase
    {
        [TestMethod]
        public void PropertyChangeBase_RaisesEventsAsExpected()
        {
            // arrange
            var lifter = new MockPropertyChangeBase
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

            // act
            lifter.NickName = "Alex";
            lifter.Height = 6.2;
            dumbellsWasObserved = lifter.RaisePropertyChangedEvent((raiseThe) => raiseThe.Dumbbells);

            // assert
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
            var mock = new MockPropertyChangeBase();
            var result = mock.GetPropertyString((vm) => vm.Dumbbells);
            Assert.AreEqual(nameof(mock.Dumbbells), result);
        }
    }

    
}
