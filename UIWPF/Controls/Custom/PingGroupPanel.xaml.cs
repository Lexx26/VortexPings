using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            if (DataItems == null)
                DataItems = new ObservableCollection<NodeGroup>();

            
            // Создание новой строки и кнопки внутри грида
            var newRow = new RowDefinition();
            newRow.Height = new GridLength(70);
            GroupGrid.RowDefinitions.Add(newRow);

            var newButton = new PingGroupButton();
            var groupNodeItem = new NodeGroup();
            groupNodeItem.Name = "Test";
            groupNodeItem.Id = 1;
            newButton.NodeGroupItem = groupNodeItem;
            newButton.ParentPingGroupPanel = this;

            DataItems.Add(groupNodeItem);


            Grid.SetRow(newButton, GroupGrid.RowDefinitions.Count - 1); // Устанавливаем новую кнопку в новую строку
            GroupGrid.Children.Add(newButton);
        }

        public void RemoveGroupe(PingGroupButton pingGroupButton)
        {
            DataItems.Remove(pingGroupButton.NodeGroupItem);

            int rowIndex = Grid.GetRow(pingGroupButton);

            // Если строка найдена, то удаляем ее и кнопку из Grid
            if (rowIndex != -1)
            {
                GroupGrid.RowDefinitions.RemoveAt(rowIndex);

                // Также удаляем кнопку из коллекции Children
                GroupGrid.Children.Remove(pingGroupButton);


                // Обновляем индексы всех остальных кнопок в Grid
                RebuildRowIndex(rowIndex);
            }
            pingGroupButton.NodeGroupItem.Dispose();
        }

        private void RebuildRowIndex(int rowIndex)
        {
            for (int i = rowIndex; i < GroupGrid.RowDefinitions.Count; i++)
            {
                foreach (UIElement child in GroupGrid.Children)
                {
                    if (Grid.GetRow(child) == i + 1)
                    {
                        Grid.SetRow(child, i);
                    }
                }
            }
        }

        public PingGroupPanel()
        {
            InitializeComponent();
        }
    }
}
