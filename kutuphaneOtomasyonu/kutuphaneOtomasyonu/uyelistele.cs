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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;


namespace kutuphaneOtomasyonu
{
    public partial class uyelistele : Form
    {
        public uyelistele()
        {
            InitializeComponent();
        }
        byte[] imageData;

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-MC1EN2U\SQLEXPRESS;Initial Catalog=Projemiz;Integrated Security=True");

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string tcNo = textBox7.Text;
                string sorgu = "select * from uyeler where tcNo='" + tcNo + "'";
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

        private void uyelistele_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlDataAdapter sorgu = new SqlDataAdapter("select * from uyeler", baglanti);
            DataTable dt = new DataTable();
            sorgu.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

           
            //byte[] imageData;
            using (MemoryStream ms = new MemoryStream())
            {
                pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                imageData = ms.ToArray();
            }

            //if (imageData.Length == null)
            //{
            //    MessageBox.Show("Lütfen bir resim ekleyin.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            //else
            {
                baglanti.Open();
                DialogResult secim = new DialogResult();
                secim = MessageBox.Show("Emin misin?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (secim == DialogResult.Yes)
                {
                    SqlCommand update = new SqlCommand("update uyeler set tcNo =@tcNo, adi =@adi, soyadi =@soyadi, telefon =@telefon, eposta =@eposta, adres =@adres, sifre =@sifre, resim=@resim where tcNo =@tcNo", baglanti);
                    update.Parameters.AddWithValue("@tcNo", textBox1.Text);
                    update.Parameters.AddWithValue("@adi", textBox2.Text);
                    update.Parameters.AddWithValue("@soyadi", textBox3.Text);
                    update.Parameters.AddWithValue("@telefon", textBox4.Text);
                    update.Parameters.AddWithValue("@eposta", textBox5.Text);
                    update.Parameters.AddWithValue("@adres", richTextBox1.Text);
                    update.Parameters.AddWithValue("@sifre", textBox6.Text);
                    update.Parameters.AddWithValue("@resim", imageData);
                    update.ExecuteNonQuery();
                    baglanti.Close();

                    MessageBox.Show("Güncellendi", "TEBRİKLER", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox4.Clear();
                    richTextBox1.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                    pictureBox1.Image = null;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            MessageBox.Show("Emin misin?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            SqlCommand delete = new SqlCommand("delete from uyeler where tcNo = @tcNo", baglanti);
            delete.Parameters.AddWithValue("@tcNo", textBox1.Text);
            delete.ExecuteNonQuery();

            SqlDataAdapter sorgu = new SqlDataAdapter("select * from uyeler", baglanti);
            DataTable dt = new DataTable();
            sorgu.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();

            MessageBox.Show("Üye silindi", "TEBRİKLER", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                using (SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-MC1EN2U\SQLEXPRESS;Initial Catalog=Projemiz;Integrated Security=True"))
                {
                    baglanti.Open();

                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                    string TcNo = selectedRow.Cells["TcNo"].Value.ToString();

                    SqlCommand sorgu = new SqlCommand("SELECT * FROM uyeler WHERE TcNo = @TcNo", baglanti);
                    sorgu.Parameters.AddWithValue("@TcNo", TcNo);

                    using (SqlDataReader okuyucu = sorgu.ExecuteReader())
                    {
                        if (okuyucu.Read())
                        {
                            textBox1.Text = okuyucu["TcNo"].ToString();
                            textBox2.Text = okuyucu["adi"].ToString();
                            textBox3.Text = okuyucu["soyadi"].ToString();
                            textBox4.Text = okuyucu["telefon"].ToString();
                            textBox5.Text = okuyucu["eposta"].ToString();
                            richTextBox1.Text = okuyucu["adres"].ToString();
                            textBox6.Text = okuyucu["sifre"].ToString();

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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
