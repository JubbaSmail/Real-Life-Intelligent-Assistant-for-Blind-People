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

namespace Main_Server
{
    public partial class MainFrm : Form
    {
        
        Commander General;
        delegate void visual(Image img);
        visual my_visual;
        //--------------------------------------
        //public static Size pic_size;
        public MainFrm()
        {
            InitializeComponent();
            //pic_size = pictureBox1.Size;
            General = new Commander(Convert.ToInt32(txt_server_port.Text), pictureBox1.Size);
            my_visual = new visual(m_Visualization_2);
        }

        public void m_Visualization(Image img)
        {
            this.Invoke(my_visual, img);
        }

        void m_Visualization_2(Image img)
        {
            pictureBox1.Image = img;
            pictureBox1.Refresh();
        }
        //--------------------------------------

        private void button1_Click(object sender, EventArgs e)
        {
            //frmMap f = new frmMap(LatitudeX, LongitudeX);
            //f.Show();
        }

        private void label_S_Click(object sender, EventArgs e)
        {
            c_Sound_Beacon.manual = true;
            c_Sound_Beacon.manula_data = new short[2] { 1, 260 };
        }

        private void label_SE_Click(object sender, EventArgs e)
        {
            c_Sound_Beacon.manual = true;
            c_Sound_Beacon.manula_data = new short[2] {1, -30 };
        }

        private void label_E_Click(object sender, EventArgs e)
        {
            c_Sound_Beacon.manual = true;
            c_Sound_Beacon.manula_data = new short[2] { 1, 10};
        }

        private void label_NE_Click(object sender, EventArgs e)
        {
            c_Sound_Beacon.manual = true;
            c_Sound_Beacon.manula_data = new short[2] { 1, 30};
        }

        private void labelN_Click(object sender, EventArgs e)
        {
            c_Sound_Beacon.manual = true;
            c_Sound_Beacon.manula_data = new short[2] { 1, 90};
        }

        private void labelNW_Click(object sender, EventArgs e)
        {
            c_Sound_Beacon.manual = true;
            c_Sound_Beacon.manula_data = new short[2] { 1, 120};
        }

        private void labelW_Click(object sender, EventArgs e)
        {
            c_Sound_Beacon.manual = true;
            c_Sound_Beacon.manula_data = new short[2] { 1, 180};
        }

        private void labelSW_Click(object sender, EventArgs e)
        {
            c_Sound_Beacon.manual = true;
            c_Sound_Beacon.manula_data = new short[2] { 1, 210};
        }
    }
}