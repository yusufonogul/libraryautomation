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
using System.IO;
using System.Data.OleDb;

namespace kutuphaneOtomasyonu
{
    public partial class kitaplistele : Form
    {
        public string yazı;
        public kitaplistele()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-MC1EN2U\SQLEXPRESS;Initial Catalog=Projemiz;Integrated Security=True");

        private void kitaplistele_Load(object sender, EventArgs e)
        {
            baglanti.Open();

            SqlCommand deneme = new SqlCommand("SELECT kitapAdi FROM emanetler", baglanti);
            SqlDataReader oku = deneme.ExecuteReader();
            int i = 0;
            string kitaplar = "";
            while(oku.Read())
            {
                i++;
                if (i > 1)
                {
                    kitaplar += ", ";
                }
                kitaplar += "'" + oku["kitapAdi"].ToString().TrimEnd() + "'";
            }
            oku.Close();
            string sorgu_string = "SELECT * FROM kitaplar";
            if (kitaplar != "")
            {
                sorgu_string += " WHERE kitapAdi NOT IN (" + kitaplar + ")";
            }
            SqlCommand sorgu_yeni = new SqlCommand(sorgu_string, baglanti);
            SqlDataReader oku1 = sorgu_yeni.ExecuteReader();
            ////SqlDataAdapter sorgu = new SqlDataAdapter("select * from kitaplar", baglanti);
            DataTable dt = new DataTable();
            dt.Load(oku1);
            ////sorgu.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                   
            try
            {
                string kitapAdi = textBox2.Text;
                string sorgu = "select * from kitaplar where kitapAdi='" + kitapAdi + "'";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                DataTable tablo = new DataTable();
                baglanti.Open();
                SqlDataReader oku = komut.ExecuteReader();
                tablo.Load(oku);
                dataGridView1.DataSource = tablo;
                oku.Close();
                baglanti.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
                if (baglanti.State == ConnectionState.Open) 
                    baglanti.Close();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] imageData;
            using (MemoryStream ms = new MemoryStream())
            {
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                ms.Position = 0;
                imageData = ms.ToArray();
            }

            using (SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-MC1EN2U\SQLEXPRESS;Initial Catalog=Projemiz;Integrated Security=True"))
            {
                baglanti.Open();

                SqlCommand update = new SqlCommand("update kitaplar set kitapAdi = @kitapAdi, yazar = @yazar, sayfasayi = @sayfasayi, baskiyili = @baskiyili, yayinEvi = @yayinEvi, dil = @dil, kategori = @kategori, resim = @resim, özet = @özet WHERE kitapAdi = @kitapAdi", baglanti);
                update.Parameters.AddWithValue("@kitapAdi", textBox1.Text);
                update.Parameters.AddWithValue("@yazar", textBox3.Text);
                update.Parameters.AddWithValue("@sayfasayi", textBox5.Text);
                update.Parameters.AddWithValue("@baskiyili", textBox6.Text);
                update.Parameters.AddWithValue("@yayinEvi", textBox4.Text);
                update.Parameters.AddWithValue("@dil", comboBox3.Text);
                update.Parameters.AddWithValue("@kategori", comboBox4.Text);
                update.Parameters.AddWithValue("@resim", imageData);
                update.Parameters.AddWithValue("@özet", textBox7.Text);

                update.ExecuteNonQuery();
                MessageBox.Show("Güncellendi", "TEBRİKLER", MessageBoxButtons.OK, MessageBoxIcon.Information);

                baglanti.Close();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            DialogResult secim = new DialogResult();
            secim = MessageBox.Show("Emin misin?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (secim == DialogResult.Yes)
            {
                SqlCommand delete = new SqlCommand("delete from kitaplar where kitapAdi = @kitapAdi", baglanti);
                delete.Parameters.AddWithValue("@kitapAdi", textBox1.Text);
                delete.ExecuteNonQuery();

                SqlDataAdapter sorgu = new SqlDataAdapter("select * from kitaplar", baglanti);
                DataTable dt = new DataTable();
                sorgu.Fill(dt);
                dataGridView1.DataSource = dt;
                baglanti.Close();

                MessageBox.Show("Kitap silindi", "TEBRİKLER", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBox1.Clear();
                textBox3.Clear();
                textBox5.Clear();
                textBox6.Clear();
                comboBox3.ResetText();
                comboBox4.ResetText();
                textBox4.Clear();
                textBox7.Clear();
                pictureBox1.Image = null;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            openFileDialog1.Filter = "Image Files | *.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                byte[] imageData;

                using (SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-MC1EN2U\SQLEXPRESS;Initial Catalog=Projemiz;Integrated Security=True"))
                {
                    baglanti.Open();

                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                    string kitapAdi = selectedRow.Cells["kitapAdi"].Value.ToString();

                    SqlCommand sorgu = new SqlCommand("SELECT * FROM kitaplar WHERE kitapAdi = @kitapAdi", baglanti);
                    sorgu.Parameters.AddWithValue("@kitapAdi", kitapAdi);

                    using (SqlDataReader okuyucu = sorgu.ExecuteReader())
                    {
                        if (okuyucu.Read())
                        {
                            textBox1.Text = okuyucu["kitapAdi"].ToString();
                            textBox3.Text = okuyucu["yazar"].ToString();
                            textBox5.Text = okuyucu["sayfasayi"].ToString();
                            textBox6.Text = okuyucu["baskiyili"].ToString();
                            textBox4.Text = okuyucu["yayinEvi"].ToString();
                            comboBox3.Text = okuyucu["dil"].ToString();
                            comboBox4.Text = okuyucu["kategori"].ToString();
                            textBox7.Text = okuyucu["özet"].ToString();

                            // Seçilen satırdaki "resim" sütunundaki değeri al
                            imageData = (byte[])okuyucu["resim"];

                            if (imageData != null && imageData.Length > 0)
                            {
                                using (MemoryStream ms = new MemoryStream(imageData))
                                {
                                    Image image = Image.FromStream(ms);
                                    pictureBox1.Image = image;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Seçilen hücrede resim bulunamadı.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Veri bulunamadı.");
                        }
                    }
                }
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
