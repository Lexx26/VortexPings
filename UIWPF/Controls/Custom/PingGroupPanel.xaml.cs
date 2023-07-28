using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using UIWPF.Commands;
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

        public static readonly DependencyProperty ClickedCroupeNodeProperty =
           DependencyProperty.Register("ClickedCroupeNode", typeof(NodeGroup), typeof(PingGroupPanel), new PropertyMetadata(null));

        public NodeGroup ClickedCroupeNode
        {
            get { return (NodeGroup)GetValue(ClickedCroupeNodeProperty); }
            set { SetValue(ClickedCroupeNodeProperty, value); }
        }

        public ObservableCollection<NodeGroup> DataItems
        {
            get { return (ObservableCollection<NodeGroup>)GetValue(DataItemsProperty); }
            set { SetValue(DataItemsProperty, value); }
        }

        private List<PingGroupButton> _pingGroupButtons = new List<PingGroupButton>();

        private ObservableCollection<NodeGroup> _previousDataItems;

        public static readonly DependencyProperty AddGroupCommandProperty =
        DependencyProperty.Register("AddGroupCommand", typeof(ICommand), typeof(PingGroupPanel), new PropertyMetadata(null));

        public ICommand AddGroupCommand
        {
            get { return (ICommand)GetValue(AddGroupCommandProperty); }
            set { SetValue(AddGroupCommandProperty, value); }
        }

        private void ExecuteAddGroupCommand(object parameter)
        {
            // Ваш код для обработки команды добавления группы
            // Здесь вы можете добавить новый элемент в коллекцию NodeGroups или выполнять другие действия
        }


       public static readonly DependencyProperty DeleteGroupCommandProperty =
       DependencyProperty.Register("DeleteGroupCommand", typeof(ICommand), typeof(PingGroupPanel), new PropertyMetadata(null));

        public ICommand DeleteGroupCommand
        {
            get { return (ICommand)GetValue(DeleteGroupCommandProperty); }
            set { SetValue(DeleteGroupCommandProperty, value); }
        }

        private void ExecuteDeleteGroupCommand(object parameter)
        {
            
        }

      public static readonly DependencyProperty EditGroupCommandProperty =
      DependencyProperty.Register("EditGroupCommand", typeof(ICommand), typeof(PingGroupPanel), new PropertyMetadata(null));

        public ICommand EditGroupCommand
        {
            get { return (ICommand)GetValue(EditGroupCommandProperty); }
            set { SetValue(EditGroupCommandProperty, value); }
        }

        private void ExecuteEditGroupCommand(object parameter)
        {

        }

        public static readonly DependencyProperty GroupCommandProperty =
        DependencyProperty.Register("GroupCommand", typeof(ICommand), typeof(PingGroupPanel), new PropertyMetadata(null));

        public ICommand GroupCommand
        {
            get { return (ICommand)GetValue(GroupCommandProperty); }
            set { SetValue(GroupCommandProperty, value); }
        }

        private void ExecuteGroupCommand(object parameter)
        {

        }

        public PingGroupPanel()
        {
            InitializeComponent();
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(DataItemsProperty, typeof(PingGroupPanel));
            dpd?.AddValueChanged(this, OnDataItemsChanged);

        }

        private void OnDataItemsChanged(object sender, EventArgs e)
        {
            if (_previousDataItems != null)
            {
                // Отписываемся от предыдущего события CollectionChanged
                _previousDataItems.CollectionChanged -= DataItems_CollectionChanged;
            }
            
            if (DataItems != null)
            {
                // Подписываемся на событие CollectionChanged для отслеживания изменений в коллекции
                DataItems.CollectionChanged += DataItems_CollectionChanged;
            }

            // Сохраняем текущее значение DataItems для будущих отписок
            _previousDataItems = DataItems;

            ChangingDataItems();
        }

        private void DataItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    foreach (var removedItem in e.OldItems)
                    {
                        DeleteNodeGroup(removedItem as NodeGroup);
                    }
                }
            }

            if(e.Action==NotifyCollectionChangedAction.Add)
            {
                if(e.NewItems !=null)
                {
                    foreach(var addedItem in e.NewItems)
                    {
                        AddGroupNode(addedItem as NodeGroup);
                    }
                }

            }

            if(e.Action == NotifyCollectionChangedAction.Reset)
            {
                while(_pingGroupButtons.Count>0)
                {
                    var pingGroupButton = _pingGroupButtons.FirstOrDefault();
                    RemoveGroup(pingGroupButton);
                }
            }
        }

        private void DeleteNodeGroup(NodeGroup nodeGroup)
        {
            if (nodeGroup == null)
                return;

            var pingGroupButton = _pingGroupButtons.FirstOrDefault(t => t.NodeGroupItem == nodeGroup);
            if (pingGroupButton != null) ;
            RemoveGroup(pingGroupButton);
        }

        private void AddGroupNode(NodeGroup addedItem)
        {
            if (addedItem == null)
                return;
            
            // Создание новой строки и кнопки внутри грида
            var newRow = new RowDefinition();
            newRow.Height = new GridLength(100);
            GroupGrid.RowDefinitions.Add(newRow);

            var newButton = new PingGroupButton();

            newButton.NodeGroupItem = addedItem;
            newButton.ParentPingGroupPanel = this;
            _pingGroupButtons.Add(newButton);
            Grid.SetColumn(newButton,0);
            Grid.SetRow(newButton, GroupGrid.RowDefinitions.Count - 1); // Устанавливаем новую кнопку в новую строку
            GroupGrid.Children.Add(newButton);

        }

        /// <summary>
        /// When main collection changed
        /// </summary>
        public void ChangingDataItems()
        {
            if(_pingGroupButtons.Count>0)
            {
                RemoveAllChildButtons();
            }

            if (DataItems == null)
                return;

            foreach (var dataItem in DataItems)
            {
                AddGroupNode(dataItem);
            }
        }

        /// <summary>
        /// Remove all child controls
        /// </summary>
        private void RemoveAllChildButtons()
        {
            while (_pingGroupButtons.Count>0)
            {
                RemoveGroup(_pingGroupButtons.First()); 
            }
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            if (DataItems == null)
                return;

            AddGroupCommand.Execute(null);

        }

        public void RemoveGroup(PingGroupButton pingGroupButton)
        {
            _pingGroupButtons.Remove(pingGroupButton);
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


        

    }
}
