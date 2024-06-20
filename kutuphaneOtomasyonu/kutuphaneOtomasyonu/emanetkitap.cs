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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace kutuphaneOtomasyonu
{
    public partial class emanetkitap : Form
    {
        String deneme = "";
        public emanetkitap()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-MC1EN2U\SQLEXPRESS;Initial Catalog=Projemiz;Integrated Security=True");

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string adi = textBox1.Text;
                string sorgu = "select * from uyeler where adi='" + adi + "'";
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
            baglanti.Open();
            SqlCommand patates = new SqlCommand("SELECT * FROM emanetler WHERE kitapAdi = @kitapAdi",baglanti);
            patates.Parameters.AddWithValue("@kitapAdi", textBox5.Text);
            SqlDataReader okuyucu = patates.ExecuteReader();
            if(okuyucu.Read())
            {
                MessageBox.Show("Bu kitap zaten emanet verilmiş.");
            }
            else
            {
                okuyucu.Close();
                string sorgu = "insert into emanetler(tcNo,uyeAdi,kitapAdi,kategori,baslangicTarih,bitisTarih) values(@tcNo,@uyeAdi,@kitapAdi,@kategori,@baslangicTarih,@bitisTarih)";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);

                komut.Parameters.AddWithValue("@tcNo", textBox2.Text);
                komut.Parameters.AddWithValue("@uyeAdi", textBox3.Text);
                komut.Parameters.AddWithValue("@kitapAdi", textBox5.Text);
                komut.Parameters.AddWithValue("@kategori", textBox6.Text);
                komut.Parameters.AddWithValue("@baslangicTarih", dateTimePicker1.Text);
                komut.Parameters.AddWithValue("@bitisTarih", dateTimePicker2.Text);

                komut.ExecuteNonQuery();
                MessageBox.Show("Emanet verildi!", "TEBRİKLER", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBox2.Clear();
                textBox3.Clear();
                textBox5.Clear();
                textBox6.Clear();

                SqlCommand goster = new SqlCommand("SELECT * FROM emanetler", baglanti);
                SqlDataAdapter adp = new SqlDataAdapter(goster);
                DataTable dt3 = new DataTable();
                adp.Fill(dt3);
                dataGridView3.DataSource = dt3;
            }
            baglanti.Close();
        }

        private void HideRowInDatabase(int id)
        {
            try
            {
                // Veritabanı bağlantısını aç
                baglanti.Open();

                // Veritabanında ilgili satırı güncelle (örnek olarak "IsHidden" adlı bir sütun ekledim)
                string updateQuery = $"UPDATE YourTableName SET IsHidden = 1 WHERE ID = {id}";
                SqlCommand command = new SqlCommand(updateQuery, baglanti);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanında bir hata oluştu: " + ex.Message);
            }
            finally
            {
                // Veritabanı bağlantısını kapat
                baglanti.Close();
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
                    string tcNo = selectedRow.Cells["tcNo"].Value.ToString();

                    SqlCommand sorgu = new SqlCommand("SELECT * FROM uyeler WHERE tcNo = @tcNo", baglanti);
                    sorgu.Parameters.AddWithValue("@tcNo", tcNo);
                    deneme = tcNo;

                    using (SqlDataReader okuyucu = sorgu.ExecuteReader())
                    {
                        if (okuyucu.Read())
                        {
                            textBox2.Text = okuyucu["tcNo"].ToString();
                            textBox3.Text = okuyucu["adi"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Veri bulunamadı.");
                        }
                    }
                }
            }
        }

        private void emanetkitap_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlDataAdapter sorgu = new SqlDataAdapter("select * from uyeler", baglanti);
            DataTable dt = new DataTable();
            sorgu.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();


            baglanti.Open();

            SqlCommand deneme = new SqlCommand("SELECT kitapAdi FROM emanetler", baglanti);
            SqlDataReader oku = deneme.ExecuteReader();
            int i = 0;
            string kitaplar = "";
            while (oku.Read())
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
            DataTable dt2 = new DataTable();
            dt2.Load(oku1);
            ////sorgu.Fill(dt);
            dataGridView2.DataSource = dt2;
            baglanti.Close();

            //baglanti.Open();
            //SqlDataAdapter sorgu2 = new SqlDataAdapter("select * from kitaplar", baglanti);
            //DataTable dt2 = new DataTable();
            //sorgu2.Fill(dt2);
            //dataGridView2.DataSource = dt2;
            //baglanti.Close();

            baglanti.Open();
            SqlDataAdapter sorgu3 = new SqlDataAdapter("select * from emanetler", baglanti);
            DataTable dt3 = new DataTable();
            sorgu3.Fill(dt3);
            dataGridView3.DataSource = dt3;
            baglanti.Close();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            DialogResult secim = new DialogResult();
            secim = MessageBox.Show("Emin misin?", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (secim == DialogResult.Yes)
            {
                SqlCommand delete = new SqlCommand("delete from emanetler where TcNo = @TcNo AND kitapAdi = @kitapAdi", baglanti);
                delete.Parameters.AddWithValue("@TcNo", textBox2.Text);
                delete.Parameters.AddWithValue("@kitapAdi", textBox5.Text);
                delete.ExecuteNonQuery();

                SqlDataAdapter sorgu = new SqlDataAdapter("select * from emanetler", baglanti);
                DataTable dt = new DataTable();
                sorgu.Fill(dt);
                dataGridView3.DataSource = dt;

                MessageBox.Show("Emanet alındı.", "TEBRİKLER", MessageBoxButtons.OK, MessageBoxIcon.Information);

                textBox2.Clear();
                textBox3.Clear();
                textBox5.Clear();
                textBox6.Clear();

            }
            baglanti.Close();
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                using (SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-MC1EN2U\SQLEXPRESS;Initial Catalog=Projemiz;Integrated Security=True"))
                {
                    baglanti.Open();

                    DataGridViewRow selectedRow = dataGridView2.Rows[e.RowIndex];
                    string kitapAdi = selectedRow.Cells["kitapAdi"].Value.ToString();

                    SqlCommand sorgu = new SqlCommand("SELECT * FROM kitaplar WHERE kitapAdi = @kitapAdi", baglanti);
                    sorgu.Parameters.AddWithValue("@kitapAdi", kitapAdi);
                    deneme = kitapAdi;

                    using (SqlDataReader okuyucu = sorgu.ExecuteReader())
                    {
                        if (okuyucu.Read())
                        {
                            textBox5.Text = okuyucu["kitapAdi"].ToString();
                            textBox6.Text = okuyucu["kategori"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Veri bulunamadı.");
                        }
                    }
                }
            }
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                using (SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-MC1EN2U\SQLEXPRESS;Initial Catalog=Projemiz;Integrated Security=True"))
                {
                    baglanti.Open();

                    DataGridViewRow selectedRow = dataGridView3.Rows[e.RowIndex];
                    string TcNo = selectedRow.Cells["TcNo"].Value.ToString();

                    SqlCommand sorgu = new SqlCommand("SELECT * FROM emanetler WHERE TcNo = @TcNo", baglanti);
                    sorgu.Parameters.AddWithValue("@TcNo", TcNo);
                    deneme = TcNo;

                    using (SqlDataReader okuyucu = sorgu.ExecuteReader())
                    {
                        if (okuyucu.Read())
                        {
                            textBox2.Text = okuyucu["TcNo"].ToString();
                            textBox3.Text = okuyucu["uyeAdi"].ToString();
                            textBox5.Text = okuyucu["kitapAdi"].ToString();
                            textBox6.Text = okuyucu["kategori"].ToString();
                            dateTimePicker1.Text = okuyucu["baslangicTarih"].ToString();
                            dateTimePicker2.Text = okuyucu["bitisTarih"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Veri bulunamadı.");
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string kitapAdi = textBox4.Text;
                string sorgu = "select * from kitaplar where kitapAdi='" + kitapAdi + "'";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                DataTable tablo = new DataTable();
                baglanti.Open();
                SqlDataReader oku = komut.ExecuteReader();
                tablo.Load(oku);
                dataGridView2.DataSource = tablo;
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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string TcNo = textBox7.Text;
                string sorgu = "select * from emanetler where TcNo='" + TcNo + "'";
                SqlCommand komut = new SqlCommand(sorgu, baglanti);
                DataTable tablo = new DataTable();
                baglanti.Open();
                SqlDataReader oku = komut.ExecuteReader();
                tablo.Load(oku);
                dataGridView3.DataSource = tablo;
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
    }
    
}
