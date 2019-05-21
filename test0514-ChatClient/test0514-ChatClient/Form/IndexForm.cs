using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test0514_ChatClient.Model.dto;

namespace test0514_ChatClient
{
    public partial class IndexForm : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream = default(NetworkStream);

        public IndexForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            Init(); //로그인 된 유저 정보로 라벨 값 세팅
            //서버 접속
            clientSocket.Connect("127.0.0.1", 8888);
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("login$"+LoginInfo.login.id + "$");
            serverStream = clientSocket.GetStream();
            serverStream.Write(outStream, 0, outStream.Length);
        }
        private void Init()
        {
            // DTO 객체에 db정보 초기화
            UserDto userDto = new UserDto();
            FriendDto dto = new FriendDto();
            userDto.Load();
            dto.Load();

            // 로그인된 유저의 정보를 폼에 띄우기
            label1.Text = LoginInfo.login.name +"  ("+LoginInfo.login.id+")";
            label2.Text =  LoginInfo.login.message;
            MemoryStream ms = new MemoryStream(LoginInfo.login.image);
            pictureBox1.Image = Image.FromStream(ms);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            
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
            if (new FriendManagerForm().ShowDialog() == DialogResult.Cancel) { Init(); LoginInfo.selectedUser = null; }
        }

        private void button3_Click(object sender, EventArgs e) // 채팅
        {
            LoginInfo.selectedUser = UserDto.Users.Find(x => x.id == listBox1.SelectedValue.ToString());
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("chat$" + LoginInfo.selectedUser.id + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            new ChatForm().Show();
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
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("logout$" + LoginInfo.login.id + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            LoginInfo.login = null;
            this.Close();
        }
    }
}
