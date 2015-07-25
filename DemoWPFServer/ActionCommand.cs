using System;
using System.Windows.Input;

namespace DemoWPFServer
{
    public class ActionCommand : ICommand
    {
        public ActionCommand(Action<object> action, Func<object, bool> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute != null ? canExecute : (o) => true;

            if (CanExecuteChanged != null)
                CanExecuteChanged(this, new EventArgs());
        }

        public ActionCommand(Action action) : this((o) => action(), (o) => true) { }

        public bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        private Action<object> action;
        private Func<object, bool> canExecute;

        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}
