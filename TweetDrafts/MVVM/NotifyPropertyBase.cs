using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TweetDrafts.MVVM
{
    public class NotifyPropertyBase : PropertyChangedBase
    {
        public bool SetProperty<T>(ref T backingField, T newValue, [CallerMemberName]string propertyName = "")
        {
            if(!object.Equals(backingField,newValue))
            {
                backingField = newValue;
                this.NotifyOfPropertyChange(propertyName);
                return true;
            }
            else return false;
        }
    }
}
