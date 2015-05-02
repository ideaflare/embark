using System;

namespace Embark.Convention.MVVM
{
    /// <summary>
    /// Basic ICommand implementation that ignores command paramater values
    /// </summary>
    public class ActionCommand : ActionCommand<Object>
    {
        /// <summary>
        /// Create a new instance of a Generic ICommand implementation
        /// </summary>
        /// <param name="action">Method to call when object is invoked</param>
        /// <param name="canExecute">Method to determine whether command can execute in its current state</param>
        public ActionCommand(Action action, Func<bool> canExecute = null)
            : base((o) => action(), (o) => canExecute == null || canExecute()) { }
    }    
}
