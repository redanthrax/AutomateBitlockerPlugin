using AutomateBitlockerPlugin.Domain.Entities;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for HistoryGrid.xaml
    /// </summary>
    public partial class HistoryGrid : UserControl {
        List<History> history = new List<History>();
        public HistoryGrid() {
            InitializeComponent();
        }

        public void LoadHistory(List<History> history) {
            HistoryData.ItemsSource = history;
        }
    }
}
