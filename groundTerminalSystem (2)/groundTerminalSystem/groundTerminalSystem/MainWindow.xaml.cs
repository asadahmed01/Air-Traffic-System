using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
using MySql.Data.MySqlClient;

namespace groundTerminalSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            //messages = new List<string>();
            Thread thfun = new Thread(startServer);
            //thfun.Name = "th";
            thfun.Start();
            //txtArea.ItemsSource = messages;
        }
        DatabaseOperations databaseOperations = new DatabaseOperations();
        List<string> messages;
        MySqlConnection cnn;
        string connetionString = null;
        public static TcpListener server = null;
        DataTable dt;
        string tailNumber;                               //7_8_2018 19:34:3
        string accelX;                                  //-0.319754
        string accelY;                                  //-0.716176
        string accelZ;                                  //1.797150
        string Weight;                                  //2154.670410
        string altitude;                                //1643.844116
        string pitch;                                   //0.022278
        string bank;
        string checkSumReceived;
        int CheckSum;
        string[] spilittedPacket = new string[12];//0.033622
        string seqNum;


        private void searchBtn(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text;
            databaseOperations.searchDtatBase(searchTerm);

        }

        private void startServer()
        {

            string threadName = Thread.CurrentThread.Name;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;
                connetionString = "server=localhost;database=flightdata;uid=root;pwd=root";

                //cnn = new MySqlConnection(connetionString);
                //ConnectToDatabase(connetionString);
                
               cnn =  databaseOperations.ConnectToDatabase(connetionString);
               cnn.Open();

                // Enter the listening loop.
                while (true)
                {
                    // Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    //MessageBox.Show(Thread.CurrentThread.Name);
                    //this.Dispatcher.Invoke((Action)(() => { txtArea.ItemsSource = ("Connected!"); })); 

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;
                    int j = 1;
                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        //check checksum is vald?..........................
                        
                        //call splitPAcket here...
                        spilittedPacket = splitPacket(data);
                   
                        //then save it to database
                        tailNumber = spilittedPacket[0];
                        //seqNum = spilittedPacket[1];
                        accelX = spilittedPacket[3];
                        accelY = spilittedPacket[4];
                        accelZ = spilittedPacket[5];
                        Weight = spilittedPacket[6];
                        altitude = spilittedPacket[7];
                        pitch = spilittedPacket[8];
                        bank = spilittedPacket[9];
                        //checkSumReceived = spilittedPacket[10];
                        string newString = tailNumber + "," + accelX + -[pitch;  ","
                        //if it is valid Display to screen
                        this.Dispatcher.Invoke((Action)(() => { txtArea.Items.Add(data); }));

                        databaseOperations.insertToAttitudeTable(tailNumber, altitude, pitch, bank);
                       databaseOperations.insertToGforceTable(tailNumber, accelX, accelY, accelZ, Weight);

                    }


                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
                //cnn.Close();
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
                //cnn.Close();
            }

            // Console.WriteLine("\nHit enter to continue...");
            //Console.Read();
        }

        private string[] splitPacket(string data)
        {

            string[] temp = data.Split(',');                //Second we split the string and get rid of the ","
            string[] result = new string[11];
            for (int i = 0; i < temp.Length; i++)
            {
                result[i] = temp[i].Trim();
            }
            checkSumReceived = result[10];

            //checkSum = Convert.ToInt32(result[8]);
            CheckSum = (int)(Convert.ToDouble(result[7]) + Convert.ToDouble(result[8]) + Convert.ToDouble(result[9])) / 3;
            if(CheckSum == Convert.ToInt32(checkSumReceived))
            {
                return result;
            }
            else
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    result[i] = "";
                }
                return result;
            }
            


        }

        

        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            //text2.Text = "Button is Checked";
            cb2.Content = "ON";
            txtArea.Visibility = Visibility.Visible;
            cb2.Background = Brushes.LightGreen;
        }
        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            //text2.Text = "Button is unchecked.";
            cb2.Content = "OFF";
            txtArea.Visibility = Visibility.Hidden;
            cb2.Background = Brushes.Red;
        }

        //private void ConnectToDatabase(string connectionString)
        //{

        //    try
        //    {
        //        cnn = new MySqlConnection(connectionString);
        //        cnn.Open();
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //        cnn.Close();
        //    }
        //}


        //public void insertToAttitudeTable(string tailNumber, string altitude, string pitch, string bank)
        //{
        //    try
        //    {
        //        string attitudeQuery = "INSERT INTO attitude(tailNumber,altitude,pitch,bank) VALUES('" + tailNumber + "','" + altitude + "','" + pitch + "','" + bank + "'); ";
        //        MySqlCommand sqlAttitudeCommand = new MySqlCommand(attitudeQuery, cnn);
        //        sqlAttitudeCommand.Prepare();
        //        sqlAttitudeCommand.ExecuteNonQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //        cnn.Close();
        //    }

        //}

        //public void insertToGforceTable(string tailNumber, string accelX, string accelY, string accelZ, string weight)
        //{
        //    try
        //    {
        //        string gForceQuery = "INSERT INTO gforce(tailNumber,accelX,accelY,accelZ,weight) VALUES('" + tailNumber + "','" + accelX + "','" + accelY + "','" + accelZ + "', '" + weight + "'); ";
        //        MySqlCommand sqlGforceCommand = new MySqlCommand(gForceQuery, cnn);
        //        sqlGforceCommand.Prepare();
        //        sqlGforceCommand.ExecuteNonQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //        cnn.Close();
        //    }
        //}


        //public void searchDtatBase(string result)
        //{
        //    try
        //    {

        //       //string connectionStr = "server=localhost;database=telemtrydb;uid=root;pwd=root";
        //        string QuerySearch = "SELECT G._timeStamp, G.tailNumber, G.accelX,G.accelY,G.accelZ,G.weight,A.altitude, A.pitch,A.bank FROM gforce G INNER JOIN attitude A ON A.aID = G.gID WHERE A.tailNumber  = ('" + result + "');";
        //        //Thread.Sleep(1000);
        //        using (cnn)
        //        {
        //            // connection.Open();
        //            using (var command = new MySqlCommand(QuerySearch, cnn))
        //            {
        //                //command.CommandText = QuerySearchalt;
        //                command.Prepare();

        //                using (var reader = command.ExecuteReader())
        //                {

        //                    while (reader.Read())
        //                    {
        //                        var col1 = reader.GetString(0);
        //                        var col2 = reader.GetString(1);
        //                        var col3 = reader.GetString(2);
        //                        var col4 = reader.GetString(3);
        //                        var col5 = reader.GetString(4);
        //                        var col6 = reader.GetString(5);
        //                        var col7 = reader.GetString(6);
        //                        var col8 = reader.GetString(7);
        //                        var col9 = reader.GetString(8);
        //                        var entireCol = col1 + ", " + col2 + ", " + col3 + ", " + col4 + ", " + col5 + ", " + col6 + ", " + col7 + ", " + col8 + ", " + col9;

        //                        this.Dispatcher.Invoke(() => { txtArea.Items.Add(entireCol); });


        //                    }
        //                }

        //            }
        //            // connection.Close();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //        cnn.Close();
        //    }
        //}

    }
}
