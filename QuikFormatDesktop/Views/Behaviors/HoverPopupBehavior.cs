using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace QuikFormatDesktop.Views.Behaviors
{
    public class HoverPopupBehavior
    {
        public static readonly DependencyProperty EnableProperty =
        DependencyProperty.RegisterAttached(
            "Enable",
            typeof(bool),
            typeof(HoverPopupBehavior),
            new PropertyMetadata(false, OnChanged));

        public static void SetEnable(DependencyObject element, bool value)
            => element.SetValue(EnableProperty, value);

        public static bool GetEnable(DependencyObject element)
            => (bool)element.GetValue(EnableProperty);

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboBoxItem item)
            {
                Popup popup = null;
                var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };

                item.MouseEnter += (s, _) =>
                {
                    timer.Start();
                };

                item.MouseLeave += (s, _) =>
                {
                    timer.Stop();
                    popup?.SetCurrentValue(Popup.IsOpenProperty, false);
                };

                timer.Tick += (s, _) =>
                {
                    timer.Stop();

                    popup = FindPopup(item);
                    if (popup != null)
                        popup.IsOpen = true;
                };
            }
        }

        private static Popup FindPopup(DependencyObject parent)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is Popup p)
                    return p;

                var result = FindPopup(child);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}
