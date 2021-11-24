using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Windows.Threading;

namespace GroundTerminalSys
{
    public class ReceiveData : MainWindow
    {

        public static TcpListener server = null;
        public string tailNumber { get; set; }
        public string accelX { get; set; }
        public string accelY { get; set; }
        public string accelZ { get; set; }
        public string Weight { get; set; }
        public string altitude { get; set; }
        public string pitch { get; set; }
        public string bank { get; set; }
        public int checkSUM { get; set; }
        public uint sequenceNumber { get; set; }
        public string[] spilittedPacket { get; set; }
        public bool check = true;

        //public ReceiveData()
        //{
        //    startConnection();
        //}

        public void startConnection()
        {


            test.Content = "connecting...";
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                test.Content = "connecting...";
                // TcpListener server = new TcpListener(port);
                server = new TcpListener(IPAddress.Any, port);
                //connectToDatabase = new DBconnection();
               // connectToDatabase.DBconnect();
                // Start listening for client requests.
                server.Start();

                //this.Dispatcher.Invoke( new Action(() => { test.Content = "Connected!!"; }));
                //test.Content = "Hello";
                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {

                    server.Start();
                    // Perform a blocking call to accept requests.

                    TcpClient client = server.AcceptTcpClient();

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;
                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        //messages.Add(data.ToString());

                        // Application.Current.Dispatcher.Invoke(new Action(() =>
                        //{
                        //    //txtArea.Items.Add(data);
                        //    test.Content = "hello";
                        // }));
                        //check checksum is vald?..........................??????????????????????????????????????????????????
                        //txtArea.Items.Add(data);


                        //if it is valid Display to screen

                        //call splitPAcket here...
                        spilittedPacket = splitPacket(data);
                        //then save it to database
                        tailNumber = spilittedPacket[0];
                        //sequenceNumber = spilittedPacket[1];

                        accelX = spilittedPacket[3];
                        accelY = spilittedPacket[4];
                        accelZ = spilittedPacket[5];
                        Weight = spilittedPacket[6];
                        altitude = spilittedPacket[7];
                        pitch = spilittedPacket[8];
                        bank = spilittedPacket[9];

                        // connectToDatabase.insertToAttitudeTable(tailNumber, altitude,pitch,bank);
                        //connectToDatabase.insertToGforceTable(tailNumber, accelX, accelY, accelZ, Weight);




                    }


                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show(e.Message);
                //connectToDatabase.Close();

            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
                // connectToDatabase.Close();
            }


        }


        private string[] splitPacket(string data)
        {

            string[] temp = data.Split(',');                //Second we split the string and get rid of the ","
            string[] result = new string[11];
            for (int i = 0; i < temp.Length; i++)
            {
                result[i] = temp[i].Trim();
            }


            //checkSum = Convert.ToInt32(result[8]);

            return result;


        }
       

    }
}

