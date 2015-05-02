using System;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using Embark.Convention;
using Embark.Convention.MVVM;

namespace TestClient.IO.TestData
{
    class Dog : DataObjectBase, IDataObject
    {
        public Dog()
        {
        }

        public string Name { get; set; }

    }
}
