using Embark.Interaction.MVVM;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;

namespace TestClient.MVVM
{
    [TestClass]
    public partial class MVVM
    {
        [TestMethod]
        public void ActionCommands_FireAsExpected()
        {
            //Arrange
            var fireBasic = new TestFire();
            var fireParameter = new TestFire();

            ICommand basicCommand = new ActionCommand(fireBasic.ExecuteNone, fireBasic.CanExecute);
            ICommand paramaterCommand = new ActionCommand<int>(fireParameter.ExecuteParam, fireParameter.CanExecuteParam);

            //Act
            basicCommand.CanExecute(null);
            basicCommand.Execute(null);
            paramaterCommand.CanExecute(1);
            paramaterCommand.Execute(2);

            //Assert
            Assert.IsTrue(fireBasic.ExecuteFired);
            Assert.IsTrue(fireBasic.TestFired);

            Assert.IsTrue(fireParameter.ExecuteFired);
            Assert.IsTrue(fireParameter.TestFired);
            Assert.AreEqual(2, fireParameter.ObjectsPassedIn.Count);
            Assert.AreEqual(3, fireParameter.ObjectsPassedIn.Sum());
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

        public List<int> ObjectsPassedIn = new List<int>();

        public void ExecuteNone()
        {
            ExecuteFired = true;
        }

        public bool CanExecute()
        {
            TestFired = true;
            return true;
        }

        public void ExecuteParam(int param)
        {
            ObjectsPassedIn.Add(param);
            ExecuteNone();
        }

        public bool CanExecuteParam(int param)
        {
            ObjectsPassedIn.Add(param);
            return CanExecute();
        }
    }
}
