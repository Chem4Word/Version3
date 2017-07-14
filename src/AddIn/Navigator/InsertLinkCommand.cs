using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chem4Word.Navigator
{
    public class InsertLinkCommand:ICommand
    {
        private Action<object> execute;
        private Predicate<object> canExecute;

        public InsertLinkCommand(Action<object> execute, Predicate<object> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            bool b = canExecute == null ? true : canExecute(parameter);
            return b;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
