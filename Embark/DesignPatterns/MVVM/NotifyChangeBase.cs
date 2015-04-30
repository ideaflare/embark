using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Embark.DesignPatterns.MVVM
{
    /// <summary>
    /// Basic implementation of INotifyPropertyChanged that Notifies clients that a property value has changed.
    /// </summary>
    public abstract class NotifyChangeBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Occurs when a property value is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Set the property and raise PropertyChanged & PropertyChanging events
        /// </summary>
        /// <typeparam name="T">Type of backing field</typeparam>
        /// <param name="backingField">Backing field of the property</param>
        /// <param name="newValue">New value to change to, if it is different.</param>
        /// <param name="propertyName">Name of the public property</param>
        /// <returns>Returns true if the property has changed, otherwise returns false.</returns>
        public bool SetProperty<T>(ref T backingField, T newValue, [CallerMemberName]string propertyName = "")
        {
            if (!object.Equals(backingField, newValue))
            {
                this.OnPropertyChanging(propertyName);
                backingField = newValue;
                this.OnPropertyChanged(propertyName);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Raise the PropertyChangedEvent of a property
        /// </summary>
        /// <param name="propertyName">Name of the public property</param>
        /// <returns>true if any observers listened to changes, otherwise false. </returns>
        public bool RaisePropertyChangedEvent([CallerMemberName]string propertyName = "")
        {
            return this.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Raise the PropertyChangingEvent of a property
        /// </summary>
        /// <param name="propertyName">Name of the public property</param>
        /// <returns>true if any observers listened to changes, otherwise false. </returns>
        public bool RaisePropertyChangingEvent([CallerMemberName]string propertyName = "")
        {
            return this.OnPropertyChanging(propertyName);
        }
          
        private bool OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            else return false;
        }

        private bool OnPropertyChanging(string propertyName)
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
                return true;
            }
            else return false;
        }        
    }
}
