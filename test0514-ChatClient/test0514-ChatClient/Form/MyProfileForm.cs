using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            // DTO 객체에 db정보 초기화
            UserDto userDto = new UserDto();
            FriendDto dto = new FriendDto();
            userDto.Load();
            dto.Load();

            // 로그인된 유저의 정보를 폼에 띄우기
            textBox5.Text = LoginInfo.login.id;
            textBox1.Text = LoginInfo.login.name;
            textBox4.Text = LoginInfo.login.message;
            MemoryStream ms = new MemoryStream(LoginInfo.login.image);
            pictureBox1.Image = Image.FromStream(ms);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button1_Click(object sender, EventArgs e) //정보 수정 버튼
        {
            //유효성 검사
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("모든 항목을 입력하세요.");
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

            // 사진파일 찾기 창 띄우기
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

            // 사용자가 선택한 사진 폼에 띄우기
            pictureBox1.Image = Bitmap.FromFile(filename);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            textBox6.Text = filename;

            //DB에 저장하는 과정
            UserDto userDto = new UserDto();
            userDto.UpdatePicture(filename);
            MessageBox.Show("사진을 성공적으로 업로드했습니다.");
        }

        private void button3_Click(object sender, EventArgs e) // 기본이미지로 변경 버튼
        {
            UserDto userDto = new UserDto();
            userDto.ResetPicture();
            textBox6.Text = "";
            Init();
        }

        private void button4_Click(object sender, EventArgs e) //닫기 버튼
        {
            this.Close();
        }
    }
}