using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace KOps.Gui
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> execute;

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> execute)
        {
            this.execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
