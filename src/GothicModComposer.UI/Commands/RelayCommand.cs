using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GothicModComposer.UI.Commands
{
    public class RelayCommand : IAsyncCommand
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Func<object, Task> _execute;
        private bool _isExecuting;

        public RelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute), "Command to execute is null");
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
            => _canExecute is null || _canExecute(parameter);

        public void Execute(object parameter)
            => ExecuteAsync(parameter).ConfigureAwait(true);

        public void RaiseCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public async Task ExecuteAsync(object parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    await _execute(parameter);
                }
                finally
                {
                    _isExecuting = false;
                }
            }

            RaiseCanExecuteChanged();
        }
    }
}