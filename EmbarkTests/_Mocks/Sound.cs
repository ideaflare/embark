using Embark.Interaction;

namespace EmbarkTests._Mocks
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

        internal static Sound GetTestSound(int echo = 2)
        {
            return new Sound
            {
                Description = RandomData.GetRandomString(),
                Quality = RandomData.Random.Next(5,95),
                Echo = new Echo
                {
                    Repetitions = echo
                }
            };
        }
    }
}
