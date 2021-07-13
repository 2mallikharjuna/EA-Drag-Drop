using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using System.Collections.Generic;
using EADragDropMVVMTest.Views;
using EADragDropMVVMTest.Commands;
using EADragDropMVVMTest.Commands.Extentions;

namespace EADragDropMVVMTest.ViewModels
{

    //<Summary>
    // UI Elements container View Model holds the collection of DragDropContainer view Model
    //</Summary>
    class ElementsContainerViewModel : BaseViewModel
    {
        private RelayCommand<object> addButtonCommand;
        private RelayCommand<object> undoButtonCommand;
        private RelayCommand<object> redoButtonCommand;

        public ICommand _leftMouseButtonDown;
        public ICommand _previewMouseMove;
        public ICommand _leftMouseButtonUp;

        private bool captured = false;
        private bool mousemove = false;

        private Stack<ICommandEx> _Undocommands = new Stack<ICommandEx>();
        private Stack<ICommandEx> _Redocommands = new Stack<ICommandEx>();

        public Point previousPosition { get; set; }

        private ObservableCollection<DragDropControlViewModel> observerableControlsCollection;


        //<Summary>
        // Having the DragDropContainer view Model collection as Observerable
        //</Summary>
        public ObservableCollection<DragDropControlViewModel> DragDropControlsCollection
        {
            get
            {
                return this.observerableControlsCollection ?? (this.observerableControlsCollection = new ObservableCollection<DragDropControlViewModel>());
            }
            set
            {
                if (value != null)
                {
                    this.observerableControlsCollection = value;                    
                    NotifyPropertyChanged("DragDropControlsCollection");
                }
            }
        }

        //<Summary>
        // Declaring Handler of the Add Button Command
        //</Summary>
        public RelayCommand<object> AddButtonCommand => this.addButtonCommand ?? (this.addButtonCommand = new RelayCommand<object>(
                                                             this.ExecuteAddButton,
                                                             (arg) => true));

        //<Summary>
        // Declaring Handler of the Undo Button Command
        //</Summary>
        public RelayCommand<object> UndoButtonCommand => this.undoButtonCommand ?? (this.undoButtonCommand = new RelayCommand<object>(
                                                             this.ExecuteUndoButton,
                                                             (arg) => true));

        //<Summary>
        // Declaring Handler of the Redo Button Command
        //</Summary>
        public RelayCommand<object> RedoButtonCommand => this.redoButtonCommand ?? (this.redoButtonCommand = new RelayCommand<object>(
                                                             this.ExecuteRedoButton,
                                                             (arg) => true));

        //<Summary>
        // AddButton Command Event Handler Implementaion
        //</Summary>
        private void ExecuteAddButton(object state)
        {
            if (state != null)
            {
                ElementsContainerViewModel vm = state as ElementsContainerViewModel;
                var dragdropCtrlviewModel = new DragDropControlViewModel();

                DragDropControlsCollection.Add(dragdropCtrlviewModel);

                ICommandEx cmd = new AddOrRemoveCommand(dragdropCtrlviewModel);
                _Undocommands.Push(cmd); 
            }
        }

        //<Summary>
        // Undo Button Command Event Handler Implementaion
        //</Summary>
        private void ExecuteUndoButton(object state)
        {
            if (state != null)
            {
                if (_Undocommands.Count != 0)
                {                                   
                    ICommandEx command = _Undocommands.Pop();
                    command.UnExecute(this);
                    _Redocommands.Push(command);                    
                }
            }
        }

        //<Summary>
        // Redo Button Command Event Handler Implementaion
        //</Summary>
        private void ExecuteRedoButton(object state)
        {
            if (state != null)
            {
                if (_Redocommands.Count != 0)
                {                 
                    ICommandEx command = _Redocommands.Pop();
                    command.Execute(this);
                    _Undocommands.Push(command);                    
                }
            }
        }

        //<Summary>
        // Mouse button down capture interaction Event Handler Implementaion
        // Capture the current postion of DragDrop control view model by reading creating the point from RectX and RectY and update to ready for Mouse event up
        //</Summary>
        public ICommand LeftMouseButtonDown
        {
            get
            {
                return _leftMouseButtonDown ?? (_leftMouseButtonDown = new RelayCommand<MouseEventArgs>(
                   e =>
                   {
                       if (captured == false)
                       {

                           Debug.WriteLine("LeftMouseButtonDown captured");
                           captured = true;
                           DragDropControlView element = e.Source as DragDropControlView;
                           var dragDropControlViewModel = element.ChildCanvas.DataContext as DragDropControlViewModel;
                           previousPosition = new Point(dragDropControlViewModel.RectX, dragDropControlViewModel.RectY);                           
                       }
                   }));
            }
        }

        //<Summary>
        // Mouse Move capture interaction Event Handler Implementaion
        // Capture the current postion of DragDrop control view model by reading creating the point from RectX and RectY and do some debug printing
        //</Summary>
        public ICommand PreviewMouseMove
        {
            get
            {
                return _previewMouseMove ?? (_previewMouseMove = new RelayCommand<MouseEventArgs>(
                   e =>
                   {
                       if (captured )
                       {
                           mousemove = true;
                           DragDropControlView element = e.Source as DragDropControlView;
                           var viewModel = element.ChildCanvas.DataContext as DragDropControlViewModel;
                           Debug.WriteLine(string.Format($"MainRectX = {viewModel.RectX} MainRectY = {viewModel.RectY}"));
                       }
                   }));
            }
        }        

        //<Summary>
        // Mouse Button up capture interaction Event Handler Implementaion
        // Create the MouseMove command and push to undoCommand colletion whenever Mouse move action completed
        // MouseMove command uses the mouse position(prev) of when mouse button down event
        //</Summary>
        public ICommand LeftMouseButtonUp
        {
            get
            {
                return _leftMouseButtonUp ?? (_leftMouseButtonUp = new RelayCommand<MouseEventArgs>(
                   e =>
                   {                       
                       if (captured == true && mousemove)
                       {                           
                           mousemove = false;
                           captured = false;
                           DragDropControlView element = e.Source as DragDropControlView;
                           var dragDropControlViewModel = element.ChildCanvas.DataContext as DragDropControlViewModel;
                           var currentPosition = new Point(dragDropControlViewModel.RectX, dragDropControlViewModel.RectY);
                           ICommandEx cmd = new MoveCommand(dragDropControlViewModel, previousPosition, currentPosition);
                           _Undocommands.Push(cmd);
                       }
                   }));
            }
        }       

    }
}
