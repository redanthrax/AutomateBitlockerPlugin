using AutomateBitlockerPlugin.Domain.Entities;
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

namespace AutomateBitlockerPlugin.AppUI.Tabs.Controls {
    /// <summary>
    /// Interaction logic for LocationGrid.xaml
    /// </summary>
    public partial class LocationGrid : UserControl {

        public ObservableCollection<Computer> items { get; set; }

        public List<Computer> encryptionList { get; set; }

        public event EventHandler NotifyBoxChecked;

        protected virtual void OnNotifyBoxChecked() {
            if (NotifyBoxChecked != null) {
                NotifyBoxChecked(this, EventArgs.Empty);
            }
        }

        public LocationGrid() {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            LocationData.ItemsSource = items;
            encryptionList = new List<Computer>();
        }

        public async Task LoadData(List<Computer> data) {
            await Task.Run(() =>
            {
                items = new ObservableCollection<Computer>(data);
            });
        }

        public async Task ClearData() {
            Dispatcher.Invoke(() =>
            {
                items.Clear();
            });
        }

        public async Task AddComputer(Computer item) {
            Dispatcher.Invoke(() =>
            {
                items.Add(item);
            });
        }

        public List<Computer> GetEncryptionComputers() {
            return encryptionList;
        }

        public List<Computer> GetCheckedItems() {
            var encryptList = new List<Computer>();
            for (int i = 0; i < LocationData.Items.Count; i++) {
                var item = LocationData.Items[i];
                var checkbox = LocationData.Columns[0].GetCellContent(item) as CheckBox;
                if ((bool)checkbox.IsChecked) {
                    encryptionList.Add((Computer)LocationData.Items[i]);
                }
            }

            return encryptionList;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) {
            var locationItem = (Computer)((CheckBox)sender).DataContext;
            encryptionList.Add(locationItem);
            OnNotifyBoxChecked();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e) {
            var locationItem = (Computer)((CheckBox)sender).DataContext;
            encryptionList.Remove(locationItem);
            OnNotifyBoxChecked();
        }

        public void RemoveComputer(Computer computer) {
            Dispatcher.Invoke(() =>
            {
                items.Remove(computer);
            });
        }
    }
}
