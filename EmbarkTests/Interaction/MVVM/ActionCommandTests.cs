using Xunit;
using Embark.Interaction.MVVM;
using System.Windows.Input;
using System.Linq;
using System;

namespace EmbarkTests.Interaction.MVVM
{
    public class ActionCommandTests
    {
        [Fact]
        public void ActionCommands_FireAsExpected()
        {
            // arrange
            var fireBasic = new _MockActionCommand();
            var fireParameter = new _MockActionCommand();

            ICommand basicCommand = new ActionCommand(fireBasic.ExecuteNone, fireBasic.CanExecute);
            ICommand valueTypeCommand = new ActionCommand<int>(fireParameter.ExecuteParam, fireParameter.CanExecuteParam);

            // act
            basicCommand.Execute(null);
            basicCommand.Execute("n/a input");
            valueTypeCommand.Execute(2);

            Assert.True(basicCommand.CanExecute(null));
            Assert.True(basicCommand.CanExecute("any input ignored"));
            Assert.True(valueTypeCommand.CanExecute(1));

            // assert
            Assert.True(fireBasic.ExecuteFired);
            Assert.True(fireBasic.TestFired);

            Assert.True(fireParameter.ExecuteFired);
            Assert.True(fireParameter.TestFired);
            Assert.Equal(2, fireParameter.ObjectsPassedIn.Count);
            Assert.Equal(3, fireParameter.ObjectsPassedIn.Sum());
        }

        [Fact]
        public void ActionCommands_InvalidInput_RespondsAsExpected()
        {
            // arrange
            var fireBasic = new _MockActionCommand();
            var fireParameter = new _MockActionCommand();

            ICommand parameterCommandInt32 = new ActionCommand<int>(fireParameter.ExecuteParam, fireParameter.CanExecuteParam);
            ICommand parameterCommandObject = new ActionCommand<object>((o) => { });
            ICommand parameterCommandObjectFunc = new ActionCommand<object>((o) => { }, (o) => { return true; });

            Assert.Throws<NullReferenceException>(() => parameterCommandInt32.CanExecute(null));

            Assert.Throws<NullReferenceException>(() => parameterCommandInt32.Execute(null));

            parameterCommandObject.Execute(null);
            parameterCommandObjectFunc.Execute(null);

            Assert.True(parameterCommandObject.CanExecute(null));
            Assert.True(parameterCommandObjectFunc.CanExecute(null));
        }
    }

    
}
