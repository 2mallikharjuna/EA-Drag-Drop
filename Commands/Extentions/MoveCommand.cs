using System;
using System.Windows;
using System.Windows.Input;
using EADragDropMVVMTest.ViewModels;

namespace EADragDropMVVMTest.Commands.Extentions
{
    /// <summary>
    /// Move Command Class
    /// </summary>
    public class MoveCommand : ICommandEx
    {        
        Point prevAnchorPoint;
        Point NextAnchorPoint;        
        DragDropControlViewModel _viewModel;

        public BaseViewModel UiElementViewModel => _viewModel;

        /// <summary>
        /// Move Command Constructor
        /// </summary>
        /// <param name="uiElement">DragDrop Control UI Element</param>
        /// <param name="prev">Prev Anchor point</param>
        /// <param name="current">Current Anchor point</param>
        public MoveCommand(DragDropControlViewModel viewModel, Point prev, Point current)
        {            
            _viewModel = viewModel;            
            prevAnchorPoint = prev;
            NextAnchorPoint = current;
        }
        /// <summary>
        /// Over ride Can Execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Redo functionality of command
        /// </summary>
        /// <param name="parameter">Observer collection</param>
        public void Execute(object parameter)
        {
            if (parameter != null)
            {
                var ctrlViewModel = parameter as ElementsContainerViewModel;                
                if (ctrlViewModel.DragDropControlsCollection.Contains(_viewModel))
                {                    
                    _viewModel.CurrentPosition = NextAnchorPoint;                    
                    //Debug.WriteLine(string.Format("Execute transformX = {0} transformY = {1} ", transform.X, transform.Y));                    
                }
            }
        }

        /// <summary>
        /// Undo Command functionality
        /// </summary>
        /// <param name="parameter">Observer collection</param>
        public void UnExecute(object parameter)
        {
            if (parameter != null)
            {
                var ctrlViewModel = parameter as ElementsContainerViewModel;
              
                if (ctrlViewModel.DragDropControlsCollection.Contains(_viewModel))
                { 
                    _viewModel.CurrentPosition = prevAnchorPoint;                    
                }
            }
        }

        // Notice here: the events should be passed to the command manager to take care about it
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
