using System;
using System.Windows.Input;

namespace StrawberryClient.Command
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        Action<object> _executeMethod;

        public RelayCommand(Action<object> executeMethod)
        {
            this._executeMethod = executeMethod;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executeMethod(parameter);
        }
    }
}
