using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using AForge.Video;
using AForge.Video.DirectShow;

namespace kutuphaneOtomasyonu
{
    public partial class uyeekle : Form
    {
        public uyeekle()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-MC1EN2U\SQLEXPRESS;Initial Catalog=Projemiz;Integrated Security=True");
        private FilterInfoCollection webcam;//webcam isminde tanımladığımız değişken bilgisayara kaç kamera bağlıysa onları tutan bir dizi. 
        private VideoCaptureDevice cam;//cam ise bizim kullanacağımız aygıt.
        private void button1_Click(object sender, EventArgs e)
        {    //Resim eklemek
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            openFileDialog1.Filter = "Image Files | *.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Kullanıcıya işlemin yapılmasını teyitlesin
            DialogResult secim = new DialogResult();
            secim = MessageBox.Show("Emin misin?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (secim == DialogResult.Yes) // evete basıldığında silinecek
                                           // hayıra basılırsa silinmeyecek
            {
                textBox1.Clear();
                textBox1.Clear();
                textBox1.Clear();
                textBox1.Clear();
                textBox1.Clear();
                richTextBox1.Clear();
                pictureBox1.Image = null;
            }
        }

            byte[] imageData;
        private void button3_Click(object sender, EventArgs e)
        {
             if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);//Picturebox'taki görüntüyü kaydediyoruz.
            }
            MessageBox.Show("Emin misin?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            using (MemoryStream ms = new MemoryStream())
            {
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                imageData = ms.ToArray();
            }
            string sorgu = "insert into uyeler(tcNo,adi,soyadi,telefon,eposta,adres,sifre,resim) values(@tcNo,@adi,@soyadi,@telefon,@eposta,@adres,@sifre,@resim)";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);

            komut.Parameters.AddWithValue("@tcNo", textBox1.Text);
            komut.Parameters.AddWithValue("@adi", textBox2.Text);
            komut.Parameters.AddWithValue("@soyadi", textBox3.Text);
            komut.Parameters.AddWithValue("@telefon", textBox4.Text);
            komut.Parameters.AddWithValue("@eposta", textBox5.Text);
            komut.Parameters.AddWithValue("@adres", richTextBox1.Text);
            komut.Parameters.AddWithValue("@sifre", textBox6.Text);
            komut.Parameters.AddWithValue("@resim", imageData);

            baglanti.Open();
            komut.ExecuteNonQuery();

            MessageBox.Show("Üye eklendi!", "TEBRİKLER", MessageBoxButtons.OK, MessageBoxIcon.Information);

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            richTextBox1.Clear();
            textBox5.Clear();
            textBox6.Clear();
            pictureBox1.Image = null;

            baglanti.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void uyeekle_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.resimsizkullanici;

            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);//webcam dizisine mevcut kameraları dolduruyoruz.
            foreach (FilterInfo videocapturedevice in webcam)
            {
                comboBox1.Items.Add(videocapturedevice.Name);//kameraları combobox a dolduruyoruz.
            }
            comboBox1.SelectedIndex = 0; //Comboboxtaki ilk index numaralı kameranın ekranda görünmesi için
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cam = new VideoCaptureDevice(webcam[comboBox1.SelectedIndex].MonikerString);
            cam.NewFrame += new NewFrameEventHandler(cam_NewFrame);
            cam.Start();
        }

        private void cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bit = (Bitmap)eventArgs.Frame.Clone();
            pictureBox2.Image = bit;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (cam.IsRunning) //kamera açıksa kapatıyoruz.
            {
                cam.Stop();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox2.Image;

            //using (MemoryStream ms = new MemoryStream())
            //{
            //    pictureBox1.Image.Save(ms, pictureBox2.Image.RawFormat);
            //    imageData = ms.ToArray();
            //}
        }

        private void uyeekle_FormClosing(object sender, FormClosingEventArgs e)
        {
            //For kapatılırken kamera açıksa kapatıyoruz.
            if (cam.IsRunning)
            {
                cam.Stop();
            }
        }
    }
}
