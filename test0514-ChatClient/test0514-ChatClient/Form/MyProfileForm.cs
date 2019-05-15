using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test0514_ChatClient.Model.dto;

namespace test0514_ChatClient
{
    public partial class MyProfileForm : Form
    {
        public MyProfileForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            Init();
        }
        private void Init()
        {
            textBox1.Text = LoginInfo.login.name;
            textBox4.Text = LoginInfo.login.name + " / " + LoginInfo.login.id;
            textBox5.Text = LoginInfo.login.id;
            if (LoginInfo.login.gender == "남자")
            {
                pictureBox1.Load(@"C:\dev\19-1modern\test0514-ChatClient\test0514-ChatClient\img\default_man.jpg");
            }
            else
            {
                pictureBox1.Load(@"C:\dev\19-1modern\test0514-ChatClient\test0514-ChatClient\img\default_woman.jpg");
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button1_Click(object sender, EventArgs e) //정보 수정 버튼
        {
            //유효성 검사
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("모든 항목을 입력하세요."+textBox2.Text);
            }
            else if (textBox2.Text != textBox3.Text) // 비밀번호 확인을 잘못 입력한 경우
            {
                MessageBox.Show("비밀번호와 비밀번호 확인을 정확히 입력하세요.");
            }
            else //모든값이 유효한 경우 중복검사
            {
                string name = textBox1.Text;
                string password = textBox2.Text;
                string msg = textBox4.Text;
                UserDto dto = new UserDto();
                dto.Load();
                dto.Update(name, password, msg);
                MessageBox.Show("정보가 수정되었습니다.");
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e) //사진 업로드 버튼
        {
            string filename = "";
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
            textBox6.Text = filename;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            
        }

        private void button3_Click(object sender, EventArgs e) //사진 삭제 버튼
        {
            textBox6.Text = "기본 이미지";
        }

        private void button4_Click(object sender, EventArgs e) //닫기 버튼
        {
            this.Close();
        }
    }
}