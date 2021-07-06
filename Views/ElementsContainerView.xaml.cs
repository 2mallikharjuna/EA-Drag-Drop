using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EADragDropMVVMTest.ViewModels;

namespace EADragDropMVVMTest.Views
{
    public partial class ElementsContainerView : UserControl
    {
        public ElementsContainerView()
        {
            InitializeComponent();
            this.DataContext = new ElementsContainerViewModel();
        }        
    }
}
