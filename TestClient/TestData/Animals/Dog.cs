using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Embark.Interfaces;
using Embark.Interaction;
using System.Windows.Input;
using Embark.DesignPatterns.MVVM;

namespace TestClient.IO.TestData
{
    class Dog : DataEntity, IDataEntity
    {
        public Dog()
        {
            this.Bark = new ActionCommand(() => { },() => true);
        }


        public ICommand Bark { get; private set; }
    }
}
