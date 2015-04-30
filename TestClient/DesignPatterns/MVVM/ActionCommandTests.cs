using Embark.DesignPatterns.MVVM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestClient.DesignPatterns.MVVM
{
    [TestClass]
    public class ActionCommandTests
    {
        [TestMethod]
        public void ActionCommands_FireAsExpected()
        {
            //Arrange
            var testFire = new TestFire();

            ICommand basicCommand = new ActionCommand(testFire.ExecuteNone, testFire.CanExecute);

            //Act
            basicCommand.CanExecute(null);
            basicCommand.Execute(null);

            //Assert
            Assert.IsTrue(testFire.ExecuteFired);
            Assert.IsTrue(testFire.TestFired);
        }


    }

    public class TestFire
    {
        public TestFire()
        {
            ExecuteFired = false;
            TestFired = false;
        }

        public bool ExecuteFired { get; private set; }
        public bool TestFired { get; set; }

        public void ExecuteNone()
        {
            ExecuteFired = true;
        }

        public bool CanExecute()
        {
            TestFired = true;
            return true;
        }
    }
}
