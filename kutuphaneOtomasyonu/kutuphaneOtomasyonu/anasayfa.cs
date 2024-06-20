using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace kutuphaneOtomasyonu
{
    public partial class anasayfa : Form
    {
        public anasayfa()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToLongDateString();
            label3.Text = DateTime.Now.ToLongTimeString();
        }

        private void button1_Click(object sender, EventArgs e)
        {           
            kitapekle f2 = new kitapekle();
            f2.ShowDialog();          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            kitaplistele f3 = new kitaplistele();
            f3.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            uyeekle f4 = new uyeekle();
            f4.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            uyelistele f5 = new uyelistele();
            f5.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            emanetkitap f6 = new emanetkitap();
            f6.ShowDialog();
        }

        private void anasayfa_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
