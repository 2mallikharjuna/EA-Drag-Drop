
using System.ComponentModel;

namespace EADragDropMVVMTest.ViewModels
{
    //<Summary>
    // Base View Model with INotifyPropertyChanged Implementation 
    //</Summary>
    public class BaseViewModel : INotifyPropertyChanged
    {       
        public event PropertyChangedEventHandler PropertyChanged;

        //<Summary>
        // INotifyPropertyChanged Implementation 
        //</Summary>
        protected void NotifyPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
