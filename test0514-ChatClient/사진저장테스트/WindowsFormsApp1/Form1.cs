using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        string filename = "";
        public Form1()
        {
            InitializeComponent();
        }

        byte[] imgBytes;

        private void button1_Click(object sender, EventArgs e)
        {
            //파일찾기 띄우기
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.InitialDirectory = @"C:\";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filename = dialog.FileName;
            }
            else
            {
                return;
            }
            pictureBox1.Image = Bitmap.FromFile(filename);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection("Server=localhost;Database=test;Uid=root;Pwd=1234;");
            MySqlCommand cmd;
            FileStream fs;
            BinaryReader br;
            try
            {
                if (filename.Length > 0)
                {
                    // 사진의 byte[]를 추출한다.
                    byte[] ImageData;
                    fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    br = new BinaryReader(fs);
                    ImageData = br.ReadBytes((int)fs.Length);
                    br.Close();
                    fs.Close();

                    // BYTE[] 변수를 db에 저장 하는 부분
                    cmd = new MySqlCommand("insert into myimages (img) values (@img)", con);
                    cmd.Parameters.Add("@img", MySqlDbType.Blob);
                    cmd.Parameters["@img"].Value = ImageData;
                    con.Open();
                    int RowsAffected = cmd.ExecuteNonQuery();
                    if (RowsAffected > 0)
                    {
                        MessageBox.Show("업로드 성공!");
                    }
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Incomplete data!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection("Server=localhost;Database=test;Uid=root;Pwd=1234;");
            MySqlCommand cmd;
            MySqlDataAdapter da;
            try
            {
                //db에 저장된 byte[]를 읽어오기
                cmd = new MySqlCommand("select img from myimages where id=22", con);
                con.Open();
                da = new MySqlDataAdapter(cmd);
                DataTable table = new DataTable();
                da.Fill(table);
                con.Close();


                // byte[]를 Image파일로 변환하기
                byte[] img = (byte[])table.Rows[0][0];
                MemoryStream ms = new MemoryStream(img);
                // Image파일을 폼에 띄우기
                pictureBox2.Image = Image.FromStream(ms);
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                da.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection("Server=localhost;Database=chat;Uid=root;Pwd=1234;");

            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM user", conn);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "user");
            conn.Close();

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                DBNull dBNull = (DBNull)r["image"];

                //byte[] asd = (byte[])r["image"];
                if (dBNull ==DBNull)
                MessageBox.Show(r["image"].GetType()+"");
            }
        }
    }
}
