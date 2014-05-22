using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IPAddressEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string _ipAddress;

        public string IPAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        private string _subnetMask;

        public string SubnetMask
        {
            get { return _subnetMask; }
            set { _subnetMask = value; }
        }



        
    }
}
