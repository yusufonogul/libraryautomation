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

namespace kutuphaneOtomasyonu
{
    public partial class kullaniciEkle : Form
    {
        public kullaniciEkle()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-MC1EN2U\SQLEXPRESS;Initial Catalog=Projemiz;Integrated Security=True");

        private void button2_Click(object sender, EventArgs e)
        {
            //Kullanıcıya işlemin yapılmasını teyitlesin
           DialogResult secim  = new DialogResult();
           secim = MessageBox.Show("Emin misin?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(secim == DialogResult.Yes) // evete basıldığında silinecek
            {                             // hayıra basılırsa silinmeyecek
                textBox1.Clear();
                textBox2.Clear();     
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                richTextBox1.Clear();
            }
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sorgu = "insert into kullanicilar(TcNo,adi,soyadi,telefon,adres,kullaniciAdi,sifre) values(@TcNo,@adi,@soyadi,@telefon,@adres,@kullaniciAdi,@sifre)";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@TcNo", textBox1.Text);
            komut.Parameters.AddWithValue("@adi", textBox2.Text);
            komut.Parameters.AddWithValue("@soyadi", textBox3.Text);
            komut.Parameters.AddWithValue("@telefon", textBox4.Text);
            komut.Parameters.AddWithValue("@adres", richTextBox1.Text);
            komut.Parameters.AddWithValue("@kullaniciAdi", textBox5.Text);
            komut.Parameters.AddWithValue("@sifre", textBox6.Text);

            baglanti.Open();
            komut.ExecuteNonQuery();
            SqlDataReader oku = komut.ExecuteReader();

            MessageBox.Show("Kaydınız yapılmışır!", "TEBRİKLER", MessageBoxButtons.OK, MessageBoxIcon.Information);

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            richTextBox1.Clear();

            baglanti.Close();

        }

    }
}
