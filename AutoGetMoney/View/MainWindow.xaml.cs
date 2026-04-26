using AutoGetMoney.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace AutoGetMoney.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // MVVM패턴으로 만들 수가 없음 ㅠ
        private void InnerScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer == null)
                return;

            bool atTop = scrollViewer.VerticalOffset == 0;
            bool atBottom = scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight;

            if ((e.Delta > 0 && atTop) || (e.Delta < 0 && atBottom))
            {
                // 이벤트를 외부로 넘기기 위해 mark as handled false
                e.Handled = true;

                // 강제로 상위 ScrollViewer에 이벤트 전달
                var parentEventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                parentEventArgs.RoutedEvent = UIElement.MouseWheelEvent;
                parentEventArgs.Source = sender;

                var parent = FindParent<ScrollViewer>(scrollViewer);
                parent?.RaiseEvent(parentEventArgs);
            }
        }

        public static T? FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject? parent = VisualTreeHelper.GetParent(child);
            while (parent != null)
            {
                if (parent is T correctlyTyped)
                    return correctlyTyped;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }
    }
}
