using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pamaxie.Wpf.Command
{
    public class AsyncDelegateCommand : ICommand
    {
        private readonly Func<CancellationToken, Task> _asyncAction;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged;

        public AsyncDelegateCommand(Func<CancellationToken, Task> execute)
            : this(execute, null)
        {
        }

        public AsyncDelegateCommand(Func<CancellationToken, Task> asyncAction,
            Predicate<object> canExecuteAction)
        {
            this._asyncAction = asyncAction;
            this._canExecute = canExecuteAction;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        public async void Execute(object cancellationToken)
        {
            if (cancellationToken is CancellationToken token)
            {
                await ExecuteAsync(token);
            }

            await ExecuteAsync(new CancellationToken());
        }

        protected virtual async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await this._asyncAction(cancellationToken);
        }
    }
}
