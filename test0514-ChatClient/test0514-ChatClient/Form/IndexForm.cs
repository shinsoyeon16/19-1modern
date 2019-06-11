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
        public static TcpClient clientSocket = new TcpClient();
        public static NetworkStream serverStream = default(NetworkStream);
        public IndexForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            Init(); //로그인 된 유저 정보로 라벨 값 세팅
            //서버접속 및 로그인정보 전달
            clientSocket.Connect("127.0.0.1", 8888);
            serverStream = clientSocket.GetStream();
            WriteToServer("login$" + LoginInfo.login.id + "$");

            //트레이아이콘 설정
            notifyIcon1.Icon = new System.Drawing.Icon(Path.GetFullPath(@"..\..\img\chat.ico"));
            notifyIcon1.Text = "챼팅챼팅";
            notifyIcon1.BalloonTipTitle = "제목인가";
            notifyIcon1.BalloonTipText = "로그인되었습니당!!";
            notifyIcon1.ShowBalloonTip(100);
            notifyIcon1.ContextMenuStrip.Items.Add("챼팅챼팅");
            notifyIcon1.ContextMenuStrip.Items.Add("도움말", null, Help_Clicked);
            notifyIcon1.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            notifyIcon1.ContextMenuStrip.Items.Add("로그아웃", null, button4_Click);
        }
        private void Init() // 창&데이터 초기화하는 메소드
        {
            // DTO 객체에 db정보 초기화
            UserDto userDto = new UserDto();
            FriendDto dto = new FriendDto();
            userDto.Load();
            dto.Load();

            // 로그인된 유저의 정보를 폼에 띄우기
            label1.Text = LoginInfo.login.name + "  (" + LoginInfo.login.id + ")";
            label2.Text = LoginInfo.login.message;
            MemoryStream ms = new MemoryStream(LoginInfo.login.image);
            pictureBox1.Image = Image.FromStream(ms);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            this.OnLoad(new EventArgs());
        }
        private  void Noti_MouseDoubleClick(object sender, MouseEventArgs e) //트레이 알림 이벤트
        {
            this.TopMost = true;
            this.TopMost = false;
        }
        private static void Help_Clicked(object sender, EventArgs e) //트레이 알림 이벤트
        {
            MessageBox.Show("도움말 메뉴 클릭", "From Tray");
        }
        public void WriteToServer(string msg) //서버 스트림에 데이터보내는 메소드
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(msg);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }
        
        private void ListBoxBinding(object sender , EventArgs e) 
        {
            listBox1.DataSource = FriendDto.Friends;
            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "id";
            listBox2.DataSource = FriendDto.Friends;
            listBox2.DisplayMember = "name";
            listBox2.ValueMember = "id";

            ChatDto cdto = new ChatDto();
            cdto.Load();
            listBox3.DataSource = ChatDto.MyChatList;
            listBox3.DisplayMember = "name\n" + "last_message\t"+"last_time";
            listBox3.ValueMember = "chat_code";
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
            //채팅방 만들기
            ChatDto cdto = new ChatDto();
            int chat_code = cdto.StartChat(LoginInfo.login.id, LoginInfo.selectedUser.id);
            new ChatForm(clientSocket, chat_code).Show();
        }
        private void button6_Click(object sender, EventArgs e) //전체유저 채팅
       {
            //WriteToServer("chatStart$" + LoginInfo.login.id + "$0$");
            //// new ChatForm().Show();
        }

        private void button2_Click(object sender, EventArgs e) //새로고침
        {
            Init();
        }
        private void button4_Click(object sender, EventArgs e) //로그아웃
        {
            //서버에 로그아웃정보 전달
            WriteToServer("logout$" + LoginInfo.login.id + "$");
            notifyIcon1.Visible = false; 
            LoginInfo.login = null;
            this.Close();
        }
        
    }
}
