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
    //class Dog : IDataEntity
    class Dog : PropertyChangeBase
    {
        public Dog()
        {
            this.Bark = new ActionCommand(() => { },() => true);

            //Embark.I
        }

        public ICommand Bark { get; private set; }


    }
}
