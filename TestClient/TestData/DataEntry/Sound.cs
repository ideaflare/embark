using System;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        public Echo Echo { get; set; }

        public byte[] Sample { get; set; }
    }
}
