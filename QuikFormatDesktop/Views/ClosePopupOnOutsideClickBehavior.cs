using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace QuikFormatDesktop.Views
{
    public class ClosePopupOnOutsideClickBehavior : Behavior<Popup>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Opened += OnPopupOpened;
            AssociatedObject.Closed += OnPopupClosed;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Opened -= OnPopupOpened;
            AssociatedObject.Closed -= OnPopupClosed;
            Unsubscribe();
        }

        private void OnPopupOpened(object sender, EventArgs e)
        {
            Window window = Window.GetWindow(AssociatedObject);
            if (window != null)
            {
                window.PreviewMouseDown += OnPreviewMouseDown;
            }
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            Window window = Window.GetWindow(AssociatedObject);
            if (window != null)
            {
                window.PreviewMouseDown -= OnPreviewMouseDown;
            }
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var hitElement = AssociatedObject.Child as UIElement;
            if (hitElement != null)
            {
                Point pos = e.GetPosition(hitElement);
                HitTestResult result = VisualTreeHelper.HitTest(hitElement, pos);

                if (result == null)
                {
                    AssociatedObject.IsOpen = false;
                }
            }
        }
    }
}
