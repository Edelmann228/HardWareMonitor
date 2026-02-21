using System;
using System.Windows.Input;

namespace HardWareMonitor.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        // Создает команду без параметров
        public RelayCommand(Action execute, Func<bool> canExecute = null)
            : this(p => execute(), p => canExecute())
        {
        }

        // Создает команду с параметром
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Определяет, может ли команда выполняться
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        // Выполняет команду
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}