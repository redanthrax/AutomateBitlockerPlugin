using AutomateBitlockerPlugin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutomateBitlockerPlugin.AppUI.Tabs.Controls {
    public partial class HistoryForm : Form {
        public HistoryForm() {
            InitializeComponent();
        }

        public void LoadData(List<History> history) {
            ((HistoryGrid)elementHost1.Child).LoadHistory(history);
        }
    }
}
