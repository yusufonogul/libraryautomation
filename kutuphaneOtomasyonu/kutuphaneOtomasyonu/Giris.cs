using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace kutuphaneOtomasyonu
{
    public partial class Giris : Form
    {
        public Giris()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-MC1EN2U\SQLEXPRESS;Initial Catalog=Projemiz;Integrated Security=True");

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="" || textBox2.Text=="")
            {
                MessageBox.Show("Lütfen boş alan bırakmayın!","UYARI",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                baglanti.Open();
                string kullaniciAdi;
                string sifre;
                kullaniciAdi = textBox1.Text;
                sifre = textBox2.Text;

                SqlCommand komut = new SqlCommand("select * from kullanicilar where kullaniciAdi='" + kullaniciAdi + "' and sifre='"+sifre+"'", baglanti);
                SqlDataReader oku = komut.ExecuteReader();

                if(oku.Read())
                {
                    anasayfa f7 = new anasayfa(); // anasayfayı açsın
                    f7.Show();
                    this.Hide();
                    textBox1.Clear();
                    textBox2.Clear();
                }
                else
                {      //bilgiler yanlışsa hata mesajı alsın
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı.Tekrar deneyin.", "HATA!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Clear();
                    textBox2.Clear();
                }

                baglanti.Close();
            }
         
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            kullaniciEkle f8 = new kullaniciEkle(); //Kullanıcı ekle formunu açsın
            f8.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit(); //Exit yazısına tıklandığında formu kapatsın
        }
    }
}
