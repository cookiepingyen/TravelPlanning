using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TravelPlanning
{
    internal class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Action action;
        public RelayCommand(Action action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action.Invoke();
        }
    }

    internal class RelayCommand<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Action<T> action;

        public RelayCommand(Action<T> action)
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;

        }

        public void Execute(object parameter)
        {
            action.Invoke((T)parameter);
        }
    }
}
