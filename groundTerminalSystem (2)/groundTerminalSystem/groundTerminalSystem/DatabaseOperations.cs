using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace groundTerminalSystem
{
    class DatabaseOperations
    {
        private MySqlConnection cnn;
        public MySqlConnection ConnectToDatabase(string connectionString)
        {

            try
            {
                cnn = new MySqlConnection(connectionString);
                //cnn.Open();
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                cnn.Close();
            }
            return cnn;
        }



        public void insertToAttitudeTable(string tailNumber, string altitude, string pitch, string bank)
        {
            try
            {
                string attitudeQuery = "INSERT INTO attitude(tailNumber,altitude,pitch,bank) VALUES('" + tailNumber + "','" + altitude + "','" + pitch + "','" + bank + "'); ";
                MySqlCommand sqlAttitudeCommand = new MySqlCommand(attitudeQuery, cnn);
                sqlAttitudeCommand.Prepare();
                sqlAttitudeCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                cnn.Close();
            }

        }


        public void insertToGforceTable(string tailNumber, string accelX, string accelY, string accelZ, string weight)
        {
            try
            {
                string gForceQuery = "INSERT INTO gforce(tailNumber,accelX,accelY,accelZ,weight) VALUES('" + tailNumber + "','" + accelX + "','" + accelY + "','" + accelZ + "', '" + weight + "'); ";
                MySqlCommand sqlGforceCommand = new MySqlCommand(gForceQuery, cnn);
                sqlGforceCommand.Prepare();
                sqlGforceCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                cnn.Close();
            }
        }

        public void searchDtatBase(string result)
        {
            
            try
            {

                //string connectionStr = "server=localhost;database=telemtrydb;uid=root;pwd=root";
                string QuerySearch = "SELECT G._timeStamp, G.tailNumber, G.accelX,G.accelY,G.accelZ,G.weight,A.altitude, A.pitch,A.bank FROM gforce G INNER JOIN attitude A ON A.aID = G.gID WHERE A.tailNumber  = ('" + result + "');";
                //Thread.Sleep(1000);
                using (cnn)
                {
                    // connection.Open();
                    using (var command = new MySqlCommand(QuerySearch, cnn))
                    {
                        //command.CommandText = QuerySearchalt;
                        command.Prepare();

                        using (var reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {
                                var col1 = reader.GetString(0);
                                var col2 = reader.GetString(1);
                                var col3 = reader.GetString(2);
                                var col4 = reader.GetString(3);
                                var col5 = reader.GetString(4);
                                var col6 = reader.GetString(5);
                                var col7 = reader.GetString(6);
                                var col8 = reader.GetString(7);
                                var col9 = reader.GetString(8);
                                var entireCol = col1 + ", " + col2 + ", " + col3 + ", " + col4 + ", " + col5 + ", " + col6 + ", " + col7 + ", " + col8 + ", " + col9;

                                //this.Dispatcher.Invoke(() => { txtArea.Items.Add(entireCol); });
                                foreach (Window window in Application.Current.Windows)
                                {
                                    if (window.GetType() == typeof(MainWindow))
                                    {
                                        (window as MainWindow).txtArea.Items.Add(entireCol); 
                                    }
                                }

                            }
                        }

                    }
                    // connection.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                cnn.Close();
            }
        }
    }
}
