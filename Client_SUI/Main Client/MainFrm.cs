using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Media;
using System.Threading;
using SpeechLib;

namespace Main_Client
{
    public partial class MainFrm : Form
    {
        public string Latitude = "0";
        public string Longitude = "0";
        List<string> lats = new List<string>();
        List<string> longs = new List<string>();

        List<string> lats_specific = new List<string>();
        List<string> longs_specific = new List<string>();

        List<Image> all_images = new List<Image>();
        SpeechLib.SpVoice flvoice = new SpeechLib.SpVoice();
        int steps_counter = 0;
        static string server_ip = "192.168.1.110";
        c_ClientTools client_tool = new c_ClientTools(server_ip, 4444);//To Server
        Commander commander = new Commander(server_ip, 4444);//To Server
        c_Sound_Beacon atr_sound_beacon = new c_Sound_Beacon();
        //--------------------------------------

        public MainFrm()
        {
            InitializeComponent();
            // Try to open the serial port
            try
            {
                serialPort1.Close();
                serialPort1.Open();
                client_tool.Connect();
                this.Text += " - Connected to Server";
                steps_counter = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                timer1.Enabled = false;
                return;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                string data = serialPort1.ReadExisting();
                if (data.Length == 0)
                {
                    txtLat.Text = "No Valid Position";
                    return;
                }
                string[] strArr = data.Split('$');
                for (int i = 0; i < strArr.Length; i++)
                {
                    string strTemp = strArr[i];
                    string[] lineArr = strTemp.Split(',');
                    if (lineArr[0] == "GPGGA")
                    {
                        //UTC Time
                        string time = lineArr[1];
                        time = time.Insert(2, " : ");
                        time = time.Insert(7, " : ");
                        string[] hh_mm_ss = time.Split(':');
                        /*if (Math.Abs(DateTime.UtcNow.Second - Convert.ToInt32(hh_mm_ss[2])) > 2)
                        {
                            txt_time.Text = "No Sync";
                            break;
                        }*/
                        //-----------------------------------
                        lineArr[4] = lineArr[4].Remove(9, 2);
                        lineArr[2] = lineArr[2].Remove(8, 2);

                        lineArr[4] = lineArr[4].Remove(5, 1);
                        lineArr[2] = lineArr[2].Remove(4, 1);

                        string second = time.Split(':')[2];
                        second = second.Remove(0, 1);
                        //send GPS data
                        if (Latitude != lineArr[2] ||
                            Longitude != lineArr[4])
                        {
                            m_to_send(Latitude, Longitude, second);
                        }
                        Latitude = lineArr[2];
                        Longitude = lineArr[4];
                        txtLat.Text = Latitude;
                        txtLong.Text = Longitude;
                        txt_time.Text = time;
                        txt_height.Text = lineArr[9];
                        txt_satellites.Text = lineArr[7];
                        steps_counter++;
                        textBox1.Text = steps_counter.ToString();
                        break;
                    }
                    else if (lineArr[0] == "GPRMC")
                    {
                        txt_Direction.Text = lineArr[8];
                        txt_Speed.Text = lineArr[7];
                    }
                }
                
            }
            else
            {
                txtLat.Text = "COM Port Closed";
            }
        }

        private void m_to_send(string lats, string longs, string second)
        {
            if(second.Contains("."))
            {
                second = second.Split('.')[0];
            }
            //Check if GPS data ok
            if (client_tool != null)
            {
                try
                {
                    client_tool.Send

                        (IO_Type.Location, Convert.ToByte(second), lats, longs);
                    object[] rec_data = client_tool.Receive();
                    IO_Type type = (IO_Type)rec_data[0];
                    if (type == IO_Type.threeD_Sound)
                    {
                        atr_sound_beacon.m_Active_Sound_Beacon((short[])rec_data[1]);
                    }
                    else
                    {
                        // registering
                    }
                }
                catch
                {
                    MessageBox.Show("Can't connect to server");
                }
            }
        }
        //----------------------------------------------------------

        private void MainFrm_Load(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
                timer1.Enabled = false;
            else
                timer1.Enabled = true;
            lats.Clear();
            longs.Clear();
            steps_counter = 0;
            txtLat.Text = "";
        }
    }
}