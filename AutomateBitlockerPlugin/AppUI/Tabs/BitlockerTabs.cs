using AutomateBitlockerPlugin.Tabs;
using LabTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutomateBitlockerPlugin.AppUI.Tabs {
    public class BitlockerTabs : ITabs {

        protected IControlCenter controlCenterHost;

        public string Name {
            get {
                return "Bitlocker";
            }
        }

        public void Initialize(IControlCenter host) {
            controlCenterHost = host;
        }

        public void ClientClose(int clientId) {
        }

        public TabPage ClientInit(int clientId) {
            /*
            TabPage clientTab = new TabPage(Name);
            clientTab.Controls.Add(new ClientTab() { Dock = DockStyle.Fill });
            return clientTab;
            */
            return null;
        }

        public void ComputerClose(int computerId) {
        }

        public TabPage ComputerInit(int computerId) {
            TabPage computerTab = new TabPage(Name);
            computerTab.Controls.Add(new ComputerTab(controlCenterHost, computerId) { Dock = DockStyle.Fill });
            return computerTab;
        }

        public void LocationClose(int locationId) {
        }

        public TabPage LocationInit(int locationId) {
            TabPage locationTab = new TabPage(Name);
            locationTab.Controls.Add(new LocationTab(controlCenterHost, locationId) { Dock = DockStyle.Fill });
            return locationTab;
        }

        public void Decommision() {
        }

        public void DeviceClose(int deviceId) {
        }

        public TabPage DeviceInit(int deviceId) {
            return null;
        }

        public void GroupClose(int groupId) {
        }

        public TabPage GroupInit(int groupId) {
            return null;
        }

        public void MonitorsClose() {
        }

        public TabPage MonitorsInit() {
            return null;
        }

        public void SearchClose() {
        }

        public TabPage SearchInit() {
            return null;
        }

        public void TicketClose(int ticketId) {
        }

        public TabPage TicketInit(int ticketId) {
            return null;
        }

        public void AlertsClose() {
        }

        public TabPage AlertsInit() {
            return null;
        }

        public void ConfigClose() {
        }

        public TabPage ConfigInit() {
            return null;
        }
    }
}
