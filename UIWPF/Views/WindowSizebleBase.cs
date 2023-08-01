using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using UIWPF.Commands;


namespace UIWPF.Views
{
    public class WindowSizebleBase:Window
    {
        private FrameworkElement _titleBar;
        private MouseButtonEventHandler _mouseButtonEventHandler;

        public ICommand MinimizeWindowCommand { get; private set; }
        public ICommand ExpandWindowCommand { get; private set; }
        public ICommand CloseWindowCommand { get; private set; }

        public WindowSizebleBase()
        {
            this.Loaded += Window_Loaded;
            this.Closing += WindowSizebleBase_Closing;

            MinimizeWindowCommand = new RelayCommand(MinimizeWindow);
            ExpandWindowCommand = new RelayCommand(ExpandWindow);
            CloseWindowCommand = new RelayCommand(CloseWindow);

            this.MaxHeight = SystemParameters.WorkArea.Height;
            this.MaxWidth = SystemParameters.WorkArea.Width;
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
                ExpandWindow(null);
                return;
            }

            DragMove();
        }

        public void CloseWindow(object obj)
        {

            this.Close();
        }

        public void ExpandWindow(object obj)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                RestoreWindow();

            }
            else
            {
                MaximizeWindow();
            }
        }

        private void MaximizeWindow()
        {
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowState = WindowState.Maximized;
            
        }

        private void RestoreWindow()
        {
            // Восстановление размеров окна
            this.ResizeMode = ResizeMode.CanResizeWithGrip;
            this.WindowState = WindowState.Normal;
      
        }

        public void MinimizeWindow(object obj)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
