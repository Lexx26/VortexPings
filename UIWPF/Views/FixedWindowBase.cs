using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UIWPF.Commands;

namespace UIWPF.Views
{
    public class FixedWindowBase:Window
    {
        private FrameworkElement _titleBar;
        private MouseButtonEventHandler _mouseButtonEventHandler;

        public ICommand CloseWindowCommand { get; private set; }

        public FixedWindowBase()
        {
            this.Loaded += Window_Loaded;
            this.Closing += WindowSizebleBase_Closing;

            CloseWindowCommand = new RelayCommand(CloseWindow);

        }

        private void WindowSizebleBase_Closing(object? sender, CancelEventArgs e)
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
            DragMove();
        }

        public void CloseWindow(object obj)
        {

            this.Close();
        }
    }
}
