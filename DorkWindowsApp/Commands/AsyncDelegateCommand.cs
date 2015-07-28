using System;
using System.ComponentModel;
using System.Windows.Input;

// ReSharper Disable All 
namespace DorkWindowsApp.Commands
{
    /// <summary>
    /// I took this from: http://www.amazedsaint.com/2010/10/asynchronous-delegate-command-for-your.html
    /// </summary>
    public class AsyncDelegateCommand : ICommand
    {
        private BackgroundWorker _worker = new BackgroundWorker();
        private Func<bool> _canExecute;
       
        public AsyncDelegateCommand(Action action, Func<bool> canExecute = null, Action<object> completed = null, Action<Exception> error = null)
        {
            _worker.DoWork += (s, e) =>
            {
                CommandManager.InvalidateRequerySuggested();
                action();
            };

            _worker.RunWorkerCompleted += (s, e) =>
            {
                if (completed != null && e.Error == null)
                {
                    completed(e.Result);
                }
                if (error != null && e.Error != null)
                {
                    error(e.Error);
                }
                CommandManager.InvalidateRequerySuggested();
            };

            _canExecute = canExecute;
        }

        public void Cancel()
        {
            if (_worker.IsBusy)
            {
                _worker.CancelAsync();
            }
        }
       
        public bool CanExecute(object parameter)
        {
            return (_canExecute == null) ? !(_worker.IsBusy) : !(_worker.IsBusy) && _canExecute();
        }
       
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _worker.RunWorkerAsync();
        }
    }
}