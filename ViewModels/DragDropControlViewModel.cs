using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using EADragDropMVVMTest.Commands;
using System.Windows.Controls;
using System.Windows;
//using System.Drawing;

namespace EADragDropMVVMTest.ViewModels
{
    //<Summary>
    // Drag drop Item control ViewModel
    //</Summary>
    public class DragDropControlViewModel : BaseViewModel, IEquatable<DragDropControlViewModel>
    {
        private bool captured = false;
        private SolidColorBrush _theBrush;
        private readonly Random rand = new Random();
        
        //Failing to update the RectX, RectY with Data binding when I do Undo
        //So added Current position to capture position
        private Point _currentPosition;        

        private double _panelX;
        private double _panelY;
        private double _rectX;
        private double _rectY;        

        public ICommand _leftMouseButtonDown;   //Mouse down command command handler     
        public ICommand _previewMouseMove;   //Mouse move command command handler  
        public ICommand _leftMouseButtonUp; //Mouse up command command handler  

        //<Summary>
        // Drag drop control constructor
        //</Summary>
        public DragDropControlViewModel()
        {            
            PanelX = rand.Next(100, 400);
            PanelY = rand.Next(100, 400);
            RectX = PanelX - 50.0;
            RectY = PanelY - 50.0;            
            _theBrush = new SolidColorBrush(Color.FromRgb((byte)rand.Next(1, 255), (byte)rand.Next(1, 255), (byte)rand.Next(1, 233))); 
        }

        //<Summary>
        // Creat the randrop color brush to fill the Ellipse
        //</Summary>
        public SolidColorBrush ColorProperty
        {
            get { return _theBrush; }
            set
            {
                _theBrush = value;
                NotifyPropertyChanged("ColorProperty");
            }
        }

        //<Summary>
        // Command Event handler to update RectX, PanelX, RectY and PanelY properties along with mouse move command
        //</Summary>
        public ICommand PreviewMouseMove
        {
            get
            {
                return _previewMouseMove ?? (_previewMouseMove = new RelayCommand(
                   x =>
                   {
                       if (captured)
                       {
                           RectX = PanelX -25;
                           RectY = PanelY -25;
                           //Debug.WriteLine(string.Format("RectX = {0} RectY = {1} PanelX ={2} PanelX ={3}", RectX, RectY, PanelX, PanelY));
                           NotifyPropertyChanged("PreviewMouseMove");
                       }
                   }));
            }
        }

        //<Summary>
        // Command Event handler to capture the mouse button up event
        //</Summary>
        public ICommand LeftMouseButtonUp
        {
            get
            {
                return _leftMouseButtonUp ?? (_leftMouseButtonUp = new RelayCommand(
                   x =>
                   {
                       captured = false;
                   }));
            }
        }

        //<Summary>
        // Command Event handler to capture the mouse button down event
        //</Summary>
        public ICommand LeftMouseButtonDown
        {
            get
            {
                return _leftMouseButtonDown ?? (_leftMouseButtonDown = new RelayCommand(
                   x =>
                   {
                       captured = true;
                   }));
            }
        }
        

        //<Summary>
        // Current Position of of control - Data binding property
        //</Summary>
        public Point CurrentPosition
        {
            get { return _currentPosition; }
            set
            {
                if (value.Equals(_currentPosition)) return;
                _currentPosition = value;

                //Update the testEllipse drawing params
                RectX = value.X;
                RectY = value.Y;

                PanelX = value.X + 25;
                PanelY = value.Y + 25;
                //Debug.WriteLine(string.Format("_currentPosition.X = {0} _currentPosition.Y = {1}", _currentPosition.X, _currentPosition.Y));
                NotifyPropertyChanged("CurrentPosition");
            }
        } 

        //<Summary>
        // RectX Data binding property
        //</Summary>
        public double RectX
        {
            get { return _rectX; }
            set
            {
                if (value.Equals(_rectX)) return;
                _rectX = value;
                _currentPosition.X = value;
                //Debug.WriteLine(string.Format("RectX = {0} RectY = {1} PanelX ={2} PanelX ={3}", RectX, RectY, PanelX, PanelY));
                NotifyPropertyChanged("RectX");
            }
        }

        //<Summary>
        // RectY Data binding property
        //</Summary>
        public double RectY
        {
            get { return _rectY; }
            set
            {
                if (value.Equals(_rectY)) return;
                _rectY = value;
                _currentPosition.Y = value;
                //Debug.WriteLine(string.Format("RectX = {0} RectY = {1} PanelX ={2} PanelX ={3}", RectX, RectY, PanelX, PanelY));
                NotifyPropertyChanged("RectY");
            }
        }

        //<Summary>
        // PanelX Data binding property
        //</Summary>
        public double PanelX
        {
            get { return _panelX; }
            set
            {
                if (value.Equals(_panelX)) return;
                _panelX = value;
                NotifyPropertyChanged("PanelX");
            }
        }

        //<Summary>
        // PanelY Data binding property
        //</Summary>
        public double PanelY
        {
            get { return _panelY; }
            set
            {
                if (value.Equals(_panelY)) return;
                _panelY = value;
                NotifyPropertyChanged("PanelY");
            }
        }

        //<Summary>
        // Logic to compare the two DragDropContolViewModel objects
        // Used Color property as unique Id 
        // We may additional property with Hash key too.
        //</Summary>
        public bool Equals(DragDropControlViewModel other)
        {
            return ColorProperty.Color == other.ColorProperty.Color;
        }
        
    }
}
