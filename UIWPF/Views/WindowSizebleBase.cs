using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace UIWPF.Views
{
    public class WindowSizebleBase:Window
    {
        private FrameworkElement _titleBar;
        private MouseButtonEventHandler _mouseButtonEventHandler;

        public WindowSizebleBase()
        {
            this.Loaded += Window_Loaded;
            this.Closing += WindowSizebleBase_Closing;
        }

        private void WindowSizebleBase_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Loaded -= Window_Loaded;
            this.Closing -= WindowSizebleBase_Closing;
            if (_mouseButtonEventHandler != null)
                this._titleBar.MouseLeftButtonDown -= _mouseButtonEventHandler;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            this._titleBar = (FrameworkElement)this.Template.FindName("TitleBar", this);
            if (null != this._titleBar)
            {
                _mouseButtonEventHandler = new MouseButtonEventHandler(Title_MouseLeftButtonDown);
                this._titleBar.MouseLeftButtonDown += _mouseButtonEventHandler;

            }

        }

        private void Title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ExpandWindow();
                return;
            }

            DragMove();
        }

        public void CloseWindow()
        {

            this.Close();
        }

        public void ExpandWindow()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;

            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        public void MinimizeWindow()
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
