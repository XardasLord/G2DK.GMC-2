using System;
using System.Windows.Input;

namespace GothicModComposer.UI.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Func<object, bool> _canExecute;

        private readonly Action<object> _execute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            if (execute is null)
                throw new ArgumentNullException(nameof(execute), "Command to execute is null");

            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
            => _canExecute is null || _canExecute(parameter);

        public void Execute(object parameter)
            => _execute(parameter);

        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}