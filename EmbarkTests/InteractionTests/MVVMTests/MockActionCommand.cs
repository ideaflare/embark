using System.Collections.Generic;

namespace EmbarkTests.InteractionTests.MVVMTests
{
    public class MockActionCommand
    {
        public MockActionCommand()
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
