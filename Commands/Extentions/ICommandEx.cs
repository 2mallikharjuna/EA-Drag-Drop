using System.Windows.Input;
using EADragDropMVVMTest.ViewModels;

namespace EADragDropMVVMTest.Commands.Extentions
{
    //<Summary>
    // This interface is to add UnExecute contract(specification) and holds the Base View model for undo command
    //</Summary>
    public interface ICommandEx : ICommand
    {        
        void UnExecute(object? parameter);

        public BaseViewModel UiElementViewModel { get;}
    }
}
