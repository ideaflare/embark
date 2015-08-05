using Microsoft.VisualStudio.TestTools.UnitTesting;
using Embark.Interaction.MVVM;
using System.Windows.Input;
using System.Linq;
using System;

namespace EmbarkTests.Interaction.MVVM
{
    [TestClass]
    public class TestActionCommand
    {
        [TestMethod]
        public void ActionCommands_FireAsExpected()
        {
            // arrange
            var fireBasic = new MockActionCommand();
            var fireParameter = new MockActionCommand();

            ICommand basicCommand = new ActionCommand(fireBasic.ExecuteNone, fireBasic.CanExecute);
            ICommand valueTypeCommand = new ActionCommand<int>(fireParameter.ExecuteParam, fireParameter.CanExecuteParam);

            // act
            basicCommand.Execute(null);
            basicCommand.Execute("n/a input");
            valueTypeCommand.Execute(2);

            Assert.IsTrue(basicCommand.CanExecute(null));
            Assert.IsTrue(basicCommand.CanExecute("any input ignored"));
            Assert.IsTrue(valueTypeCommand.CanExecute(1));

            // assert
            Assert.IsTrue(fireBasic.ExecuteFired);
            Assert.IsTrue(fireBasic.TestFired);

            Assert.IsTrue(fireParameter.ExecuteFired);
            Assert.IsTrue(fireParameter.TestFired);
            Assert.AreEqual(2, fireParameter.ObjectsPassedIn.Count);
            Assert.AreEqual(3, fireParameter.ObjectsPassedIn.Sum());        
        }

        [TestMethod]
        public void ActionCommands_InvalidInput_RespondsAsExpected()
        { 
            // arrange
            var fireBasic = new MockActionCommand();
            var fireParameter = new MockActionCommand();

            ICommand parameterCommandInt32 = new ActionCommand<int>(fireParameter.ExecuteParam, fireParameter.CanExecuteParam);
            ICommand parameterCommandObject = new ActionCommand<object>((o) => { });
            ICommand parameterCommandObjectFunc = new ActionCommand<object>((o) => { }, (o) => { return true; });
            
            // assert
            try
            {
                parameterCommandInt32.CanExecute(null);
                Assert.Fail("Expected error");
            }
            catch (NullReferenceException) { }
            try
            {
                parameterCommandInt32.Execute(null);
                Assert.Fail("Expected error");
            }
            catch (NullReferenceException) { }

            parameterCommandObject.Execute(null);
            parameterCommandObjectFunc.Execute(null);

            Assert.IsTrue(parameterCommandObject.CanExecute(null));
            Assert.IsTrue(parameterCommandObjectFunc.CanExecute(null));
        }
    }

    
}
