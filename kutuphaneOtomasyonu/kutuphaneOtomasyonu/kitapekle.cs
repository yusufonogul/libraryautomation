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
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using AForge.Video.DirectShow;
using AForge.Video;

namespace kutuphaneOtomasyonu
{
    public partial class kitapekle : Form
    {
        public kitapekle()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-MC1EN2U\SQLEXPRESS;Initial Catalog=Projemiz;Integrated Security=True");
        private FilterInfoCollection webcam;//webcam isminde tanımladığımız değişken bilgisayara kaç kamera bağlıysa onları tutan bir dizi. 
        private VideoCaptureDevice cam;//cam ise bizim kullanacağımız aygıt.

        private void button2_Click(object sender, EventArgs e)

            //Kullanıcının sisteme resim yüklemesi
        {
            pictureBox1.SizeMode= PictureBoxSizeMode.StretchImage;
            openFileDialog1.Filter = "Image Files | *.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)

            //Kullanıcıya işlemin yapılmasını teyitlesin
        {
             if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);//Picturebox'taki görüntüyü kaydediyoruz.
            }
            MessageBox.Show("Emin misin?", "UYARI", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            byte[] imageData;
            using (MemoryStream ms = new MemoryStream())
            {
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                imageData = ms.ToArray();
            }
            string sorgu = "insert into kitaplar(kitapAdi,yazar,sayfasayi,baskiyili,yayinEvi,dil,kategori,resim,özet) values(@kitapAdi,@yazar,@sayfasayi,@baskiyili,@yayinEvi,@dil,@kategori,@resim,@özet)";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);

            komut.Parameters.AddWithValue("@kitapAdi", textBox1.Text);
            komut.Parameters.AddWithValue("@yazar", textBox3.Text);
            komut.Parameters.AddWithValue("@sayfasayi", textBox5.Text);
            komut.Parameters.AddWithValue("@baskiyili", textBox6.Text);
            komut.Parameters.AddWithValue("@yayinEvi", textBox4.Text);
            komut.Parameters.AddWithValue("@dil", comboBox3.Text);
            komut.Parameters.AddWithValue("@kategori", comboBox4.Text);
            komut.Parameters.AddWithValue("@resim", imageData);
            komut.Parameters.AddWithValue("@özet", textBox2.Text);

            baglanti.Open();
            komut.ExecuteNonQuery();
            
            MessageBox.Show("Kitap eklendi!", "TEBRİKLER", MessageBoxButtons.OK, MessageBoxIcon.Information);

            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox2.Clear();
            comboBox3.ResetText();
            comboBox4.ResetText();
            textBox6.Clear();
            textBox5.Clear();
            pictureBox1.Image = null;

            baglanti.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {    //Kullanıcıya işlemin yapılmasını teyitlesin
            DialogResult secim = new DialogResult();
            secim = MessageBox.Show("Emin misin?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (secim == DialogResult.Yes) // evete basıldığında silinecek
                                           // hayıra basılırsa silinmeyecek
            {
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                comboBox3.ResetText();
                comboBox4.ResetText();
                textBox5.Clear();
                textBox6.Clear();
                pictureBox1.Image = null;

            }
        }

        private void txtBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void kitapekle_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.resimsizkullanici;

            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);//webcam dizisine mevcut kameraları dolduruyoruz.
            foreach (FilterInfo videocapturedevice in webcam)
            {
                comboBox1.Items.Add(videocapturedevice.Name);//kameraları combobox a dolduruyoruz.
            }
            comboBox1.SelectedIndex = 0; //Comboboxtaki ilk index numaralı kameranın ekranda görünmesi için

        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = pictureBox2.Image;
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

        private void kitapekle_FormClosing(object sender, FormClosingEventArgs e)
        {
            //For kapatılırken kamera açıksa kapatıyoruz.
            if (cam.IsRunning)
            {
                cam.Stop();
            }
        }
    }
}
