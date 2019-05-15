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
    public partial class IndexForm : Form
    {
        public IndexForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            Init(); //로그인 된 유저 정보로 라벨 값 세팅
            
        }
        private void Init()
        {
            label1.Text = LoginInfo.login.name;
            label2.Text =  LoginInfo.login.gender + " / " + LoginInfo.login.id;
            if (LoginInfo.login.gender == "남자")
            {
                pictureBox1.Load(@"C:\dev\19-1modern\test0514-ChatClient\test0514-ChatClient\img\default_man.jpg");
            } else
            {
                pictureBox1.Load(@"C:\dev\19-1modern\test0514-ChatClient\test0514-ChatClient\img\default_woman.jpg");
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            FriendDto dto = new FriendDto();
            dto.Load();

            this.OnLoad(new EventArgs());
        }

        private void FriendListBinding(object sender , EventArgs e)
        {
            listBox1.DataSource = FriendDto.Friends;
            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "id";
            listBox2.DataSource = FriendDto.Friends;
            listBox2.DisplayMember = "name";
            listBox2.ValueMember = "id";
        }
        private void button1_Click(object sender, EventArgs e) //프로필관리
        {
            if (new MyProfileForm().ShowDialog() == DialogResult.Cancel) Init();
        }
        private void button5_Click(object sender, EventArgs e) //친구 관리
        {
            if (new FriendManagerForm().ShowDialog() == DialogResult.Cancel) Init();
        }

        private void button3_Click(object sender, EventArgs e) // 채팅
        {
        }
        private void button6_Click(object sender, EventArgs e) //전체유저 채팅
        {
            new ChatForm().Show();
        }

        private void button2_Click(object sender, EventArgs e) //새로고침
        {
            Init();
        }
        private void button4_Click(object sender, EventArgs e) //로그아웃
        {
            LoginInfo.login = null;
            this.Close();
        }
    }
}
