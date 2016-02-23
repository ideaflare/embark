using Xunit;
using Embark.Interaction.MVVM;

namespace EmbarkTests.Interaction.MVVM
{
    public class PropertyChangeBaseTests
    {
        [Fact]
        public void PropertyChangeBase_RaisesEventsAsExpected()
        {
            // arrange
            var lifter = new _MockPropertyChangeBase
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
            bool dumbellsUpdatedWithTrigger = false;

            lifter.TriggerWhenPropertyChanged(
                (bro) => bro.Dumbbells,
                () =>
                {
                    dumbellsUpdatedWithTrigger = true;
                });

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
            Assert.True(nameChanged);
            Assert.True(nameChanging);
            Assert.Equal("Alex", lifter.NickName);

            Assert.False(heightChanged);
            Assert.False(heightChanging);
            Assert.Equal(6.2, lifter.Height);

            Assert.True(dumbbellsraised);
            Assert.Equal(2, lifter.Dumbbells);

            Assert.True(dumbellsWasObserved);
            Assert.True(dumbellsUpdatedWithTrigger);
        }

        [Fact]
        public void GetPropertyString_ReturnsSameName()
        {
            var mock = new _MockPropertyChangeBase();
            var result = mock.GetPropertyString((vm) => vm.Dumbbells);
            Assert.Equal(nameof(mock.Dumbbells), result);
        }
    }

    
}
