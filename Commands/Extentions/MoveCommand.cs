using EADragDropMVVMTest.ViewModels;
using EADragDropMVVMTest.Views;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace EADragDropMVVMTest.Commands.Extentions
{
    /// <summary>
    /// Move Command Class
    /// </summary>
    public class MoveCommand : ICommandEx
    {
        private DragDropControlView _uiElement;
        Point prevAnchorPoint;
        Point NextAnchorPoint;        

        public BaseViewModel UiElementViewModel => _uiElement.DataContext as DragDropControlViewModel;

        /// <summary>
        /// Move Command Constructor
        /// </summary>
        /// <param name="uiElement">DragDrop Control UI Element</param>
        /// <param name="prev">Prev Anchor point</param>
        /// <param name="current">Current Anchor point</param>
        public MoveCommand(DragDropControlView uiElement, Point prev, Point current)
        {
            _uiElement = uiElement;
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
                var dragDropControlViewModel = _uiElement.DataContext as DragDropControlViewModel;
                if (ctrlViewModel.DragDropControlsCollection.Contains(dragDropControlViewModel))
                {
                    var transform = new TranslateTransform();
                    var element = VisualTreeHelper.GetParent(_uiElement) as UIElement;
                    var currentAnchorPoint = new Point(dragDropControlViewModel.RectX, dragDropControlViewModel.RectY);                    
                    
                    transform.X += NextAnchorPoint.X - currentAnchorPoint.X;
                    transform.Y += NextAnchorPoint.Y - currentAnchorPoint.Y;

                    element.RenderTransform = transform;
                    
                    Debug.WriteLine(string.Format("Execute transformX = {0} transformY = {1} ", transform.X, transform.Y));                    
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
                var dragDropControlViewModel = _uiElement.DataContext as DragDropControlViewModel;
                if (ctrlViewModel.DragDropControlsCollection.Contains(dragDropControlViewModel))
                {
                    var transform = new TranslateTransform();
                    var element = VisualTreeHelper.GetParent(_uiElement) as UIElement;
                    var currentAnchorPoint = new Point(dragDropControlViewModel.RectX, dragDropControlViewModel.RectY);
                    
                    transform.X -= currentAnchorPoint.X - prevAnchorPoint.X;
                    transform.Y -= currentAnchorPoint.Y - prevAnchorPoint.Y;
                    element.RenderTransform = transform;
                    
                    dragDropControlViewModel.CurrentPosition = prevAnchorPoint;                    

                    Debug.WriteLine(string.Format("UnExecute transformX = {0} transformY = {1} ", transform.X, transform.Y));
                    
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
