using System;
using System.Windows.Input;
using EADragDropMVVMTest.ViewModels;

namespace EADragDropMVVMTest.Commands.Extentions
{
    class AddOrRemoveCommand : ICommandEx
    {
        DragDropControlViewModel _viewModel;       
        public AddOrRemoveCommand(DragDropControlViewModel viewModel)
        {
            _viewModel = viewModel;           
        }

        public BaseViewModel UiElementViewModel => _viewModel;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        #region ICommand Members        

        public void Execute(object parameter)
        {
            if (parameter != null)
            {
                var ctrlViewModel = parameter as ElementsContainerViewModel;
                if (!ctrlViewModel.DragDropControlsCollection.Contains(_viewModel))
                {
                    ctrlViewModel.DragDropControlsCollection.Add(_viewModel);
                }
            }
        }

        public void UnExecute(object parameter)
        {
            if (parameter != null)
            {
                var ctrlViewModel = parameter as ElementsContainerViewModel;
                
                if (ctrlViewModel.DragDropControlsCollection.Contains(_viewModel))
                {
                    _viewModel.RectX = _viewModel.CurrentPosition.X;
                    _viewModel.RectY = _viewModel.CurrentPosition.Y;

                    _viewModel.PanelX = _viewModel.RectX + 25;
                    _viewModel.PanelY = _viewModel.RectY + 25;
                    ctrlViewModel.DragDropControlsCollection.Remove(_viewModel);
                    
                }
                
            }
        }

        #endregion
    }
}
