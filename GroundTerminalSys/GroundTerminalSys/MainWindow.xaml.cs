using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace GroundTerminalSys
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ReceiveData startServer = null;
        DBconnection connectToDatabase = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void searchBtn(object sender, RoutedEventArgs e)
        {

            //string result = txtSearch.Text;
            //DBconnection searchDBTable = new DBconnection();
            //searchDBTable.DBconnect();
            //searchDBTable.searchDtatBase(result);
        }
        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            //text2.Text = "Button is Checked";
            cb2.Content = "ON";
            txtArea.Visibility = Visibility.Visible;
            //XYT.Start();
        }
        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            //text2.Text = "Button is unchecked.";
            cb2.Content = "OFF";
            txtArea.Visibility = Visibility.Hidden;
        }
        private void start(object sender, RoutedEventArgs e)
        {
            //connectToDatabase = new DBconnection();
            test.Content = "before connecting";
            startServer = new ReceiveData();
            startServer.startConnection();
            //Thread tb = new Thread(() => connectToDatabase = new DBconnection());
           // tb.SetApartmentState(ApartmentState.STA);
            //Thread th = new Thread(() => startServer = new ReceiveData());
            //th.SetApartmentState(ApartmentState.STA);
            //tb.Start();
            //th.Start();
            

        }
    }
}
