using System;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using Embark.Interaction;

namespace TestClient.IO.TestData
{
    class Shoe : DataEntryBase, IDataEntry
    {
        public Shoe()
        {
        }

        public string Name { get; set; }
        public int Cost { get; set; }

    }
}
