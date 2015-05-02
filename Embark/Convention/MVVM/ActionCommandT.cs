using System;
using System.Windows.Input;

namespace Embark.Convention.MVVM
{
    /// <summary>
    /// Generic ICommand Implementation
    /// </summary>
    /// <typeparam name="T">Type of parameter that could be passed to <see cref="Execute"/> or <see cref="CanExecute"/> methods</typeparam>
    public class ActionCommand<T> : ICommand
    {
        /// <summary>
        /// Create a new instance of a Generic ICommand implementation
        /// </summary>
        /// <param name="action">Method to call when object is invoked</param>
        /// <param name="canExecute">Method to determine whether command can execute in its current state</param>
        public ActionCommand(Action<T> action, Func<T, bool> canExecute = null)
        {
            this.objectAction = action;
            this.canExecute = canExecute != null ? canExecute : (o) => true;

            if (this.CanExecuteChanged != null)
                this.CanExecuteChanged(this, new EventArgs());
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        private Action<T> objectAction;
        private Func<T, bool> canExecute;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return canExecute((T)parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            objectAction((T)parameter);
        }
    }
}
