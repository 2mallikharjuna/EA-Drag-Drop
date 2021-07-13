using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace EADragDropMVVMTest.Commands
{
    /// <summary>
    /// Interactive behaviour to capture the Move Commands
    /// </summary>
    public class UIElementDragDropBehavior : Behavior<UIElement>
    {
        /// <summary>
        /// Register the Dependency property MouseX
        /// </summary>
        public static readonly DependencyProperty MouseXProperty = DependencyProperty.RegisterAttached(
           "MouseX", typeof(double), typeof(UIElementDragDropBehavior), new PropertyMetadata(default(double)));
        /// <summary>
        /// Register the Dependency property MouseY
        /// </summary>
        public static readonly DependencyProperty MouseYProperty = DependencyProperty.RegisterAttached(
          "MouseY", typeof(double), typeof(UIElementDragDropBehavior), new PropertyMetadata(default(double)));

        
        /// <summary>
        /// Over ride functionality when Control added to elements container collection
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseLeftButtonDown += AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseLeftButtonUp;
            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
        }

        /// <summary>
        /// Over ride functionality when Control removed from elements container collection
        /// </summary>
        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonDown -= AssociatedObject_MouseLeftButtonDown;
            AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseLeftButtonUp;
            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
        }
        /// <summary>
        /// Get or Set MouseY property
        /// </summary>
        public double MouseY
        {
            get { return (double)GetValue(MouseYProperty); }
            set { SetValue(MouseYProperty, value); }
        }
        /// <summary>
        /// Get or sets MouseY property
        /// </summary>
        public double MouseX
        {
            get { return (double)GetValue(MouseXProperty); }
            set { SetValue(MouseXProperty, value); }
        }
        /// <summary>
        /// Mouse Left button down event handler
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="mouseEventArgs">mouseEventArgs object</param>
        private void AssociatedObject_MouseLeftButtonDown(object sender, MouseEventArgs mouseEventArgs)
        {
            var pos = mouseEventArgs.GetPosition(AssociatedObject);
            MouseX = pos.X;
            MouseY = pos.Y;
            AssociatedObject.CaptureMouse();
        }
        /// <summary>
        /// Mouse Left button up event handler
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="mouseEventArgs">mouseEventArgs object</param>
        private void AssociatedObject_MouseLeftButtonUp(object sender, MouseEventArgs mouseEventArgs)
        {
            // release this control.
            AssociatedObject.ReleaseMouseCapture();
        }

        /// <summary>
        /// Mouse Move button event handler
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="mouseEventArgs">mouseEventArgs object</param>
        private void AssociatedObject_MouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (AssociatedObject.IsMouseCaptured)
            {
                // get the parent container
                var container = VisualTreeHelper.GetParent(AssociatedObject) as UIElement;

                // get the position within the container
                var mousePosition = mouseEventArgs.GetPosition(container);
                MouseX = mousePosition.X;
                MouseY = mousePosition.Y;

                // move the usercontrol.
                AssociatedObject.RenderTransform = new TranslateTransform(mousePosition.X - MouseX, mousePosition.Y - MouseY);                
            }
        }

    }
}
