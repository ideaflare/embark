using Embark.Interaction;
using TestClient.TestData.Basic;

namespace TestClient.TestData.DataEntry
{
    class Sound : DataEntryBase, IDataEntry
    {
        public Sound()
        {
        }

        public string Description { get; set; }
        public int Quality { get; set; }

        private int amplitude;
        public int Amplitude
        {
            get { return amplitude; }
            set { SetProperty(ref amplitude, value); }
        }

        public Echo Echo { get; set; }

        public byte[] Sample { get; set; }
    }
}
