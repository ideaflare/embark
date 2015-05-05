using System;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using Embark.Interaction;

namespace TestClient.IO.TestData
{
    class Echo : DataEntryBase, IDataEntry
    {
        public Echo()
        {
        }

        public string Sound { get; set; }
        public int Quality { get; set; }

    }
}
