using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VortexPings.Models;

namespace UIWPF.Controls.Custom
{
    /// <summary>
    /// Interaction logic for PingGroupPanel.xaml
    /// </summary>
    public partial class PingGroupPanel : UserControl
    {
        public static readonly DependencyProperty DataItemsProperty =
           DependencyProperty.Register("DataItems", typeof(ObservableCollection<NodeGroup>), typeof(PingGroupPanel), new PropertyMetadata(null));

        public ObservableCollection<NodeGroup> DataItems
        {
            get { return (ObservableCollection<NodeGroup>)GetValue(DataItemsProperty); }
            set { SetValue(DataItemsProperty, value); }
        }

        public static readonly DependencyProperty AddRowCommandProperty =
           DependencyProperty.Register("AddGroupCommand", typeof(ICommand), typeof(PingGroupPanel), new PropertyMetadata(null));

        public ICommand AddGroupCommand
        {
            get { return (ICommand)GetValue(AddRowCommandProperty); }
            set { SetValue(AddRowCommandProperty, value); }
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            // Создание новой строки и кнопки внутри грида
            var newRow = new RowDefinition();
            newRow.Height = new GridLength(50);
            GroupGrid.RowDefinitions.Add(newRow);

            var newButton = new Button();
            newButton.Content = "New Button";
         
            Grid.SetRow(newButton, GroupGrid.RowDefinitions.Count - 1); // Устанавливаем новую кнопку в новую строку
            GroupGrid.Children.Add(newButton);
        }

        public PingGroupPanel()
        {
            InitializeComponent();
        }
    }
}
