using Embark.Interaction.MVVM;

namespace EmbarkTests.InteractionTests.MVVMTests
{
    public class MockPropertyChangeBase : PropertyChangeBase
    {
        public int Dumbbells { get; set; }

        private string nickName = "";
        public string NickName
        {
            get { return nickName; }
            set { SetProperty(ref nickName, value); }
        }

        private double height;
        public double Height
        {
            get { return height; }
            set { SetProperty(ref height, value); }
        }
    }
}
