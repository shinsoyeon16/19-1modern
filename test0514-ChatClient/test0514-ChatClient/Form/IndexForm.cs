using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using test0514_ChatClient.Model.dto;

namespace test0514_ChatClient
{
    public partial class IndexForm : Form
    {
        public static TcpClient clientSocket = new TcpClient();
        public static NetworkStream serverStream = default(NetworkStream);
        MyThread OnlineUserInformation;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        
        static UserDto udto = new UserDto();
        static FriendDto fdto = new FriendDto();
        static ChatDto cdto = new ChatDto();
        public IndexForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            //서버접속 및 로그인정보 전달
            clientSocket = new TcpClient();
            clientSocket.Connect("127.0.0.1", 8888);
            serverStream = clientSocket.GetStream();
            WriteToServer("login$" + LoginInfo.login.id + "$");
            byte[] bytesFrom = new byte[(int)clientSocket.ReceiveBufferSize];
            serverStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
            string dataFromServer = System.Text.Encoding.UTF8.GetString(bytesFrom);
            string[] data = dataFromServer.Split('$');
            if (data[0] == "Not Allowed Access")
            {
                MessageBox.Show("로그인 불가!  " + data[0]);
                Application.ExitThread();
                Environment.Exit(0);
            }

            Init(); //로그인 된 유저 정보로 라벨 값 세팅
            OnlineUserInformation = new MyThread();
            OnlineUserInformation.Start(listBox1, listBox2);
            timer.Interval = 1000;
            timer.Tick += new EventHandler(KeepChatList);
            timer.Start();
            //  this.OnLoad(new EventArgs());


            //트레이아이콘 설정    안되면 윈도우 설정 시스템 알림켜기 
            notifyIcon1.Icon = new System.Drawing.Icon(Path.GetFullPath(@"..\..\img\chat.ico"));
            notifyIcon1.Text = "챼팅챼팅";
            notifyIcon1.ShowBalloonTip(1000, "챼팅챼팅", LoginInfo.login.name + "님 환영합니다.", ToolTipIcon.None); //윈도우 자체 알림설정때문에 밀리초 안들음
            notifyIcon1.ContextMenuStrip.Items.Add("챼팅챼팅");
            notifyIcon1.ContextMenuStrip.Items.Add("도움말", null, Help_Clicked);
            notifyIcon1.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            notifyIcon1.ContextMenuStrip.Items.Add("로그아웃", null, button4_Click);
        }
        private void Init() // 창&데이터 초기화하는 메소드
        {
            // DTO 객체에 db정보 초기화
            udto.Load(); fdto.Load();

            // 로그인된 유저의 정보를 폼에 띄우기
            label1.Text = LoginInfo.login.name + "  (" + LoginInfo.login.id + ")";
            label2.Text = LoginInfo.login.message;
            MemoryStream ms = new MemoryStream(LoginInfo.login.image);
            pictureBox1.Image = Image.FromStream(ms);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void Noti_MouseDoubleClick(object sender, MouseEventArgs e) //트레이 알림 이벤트
        {
            this.TopMost = true; this.TopMost = false;
        }
        private static void Help_Clicked(object sender, EventArgs e) //트레이 알림 이벤트
        {
            MessageBox.Show("도움말 메뉴 클릭", "From Tray");
        }
        public void WriteToServer(string msg) //서버 스트림에 데이터보내는 메소드
        {
            byte[] outStream = System.Text.Encoding.UTF8.GetBytes(msg);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }

        public void ListBoxBinding(object sender, EventArgs e)
        {
            //listBox1.DataSource = FriendDto.Online_Friends;
            //listBox1.DisplayMember = "name";
            //listBox1.ValueMember = "id";
            //listBox2.DataSource = FriendDto.Offline_Friends;
            //listBox2.DisplayMember = "name";
            //listBox2.ValueMember = "id";
            //cdto.Load();
            //listBox3.DataSource = ChatDto.MyChatList;
            //listBox3.DisplayMember = "chat_display";
            //listBox3.ValueMember = "chat_users";
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
            try
            {
                LoginInfo.selectedUser = UserDto.Users.Find(x => x.id == listBox1.SelectedValue.ToString());
                int chat_code = -1;
                //1 디비에 기존 채팅방이 있는지 확인후 있으면 채팅창 열기
                if (ChatDto.MyChatList.Exists(x => x.chat_users == LoginInfo.selectedUser.id))
                {
                    chat_code = ChatDto.MyChatList.Find(x => x.chat_users == LoginInfo.selectedUser.id).chat_code;
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Name == "ChatForm" && f.Tag.ToString() == chat_code.ToString())
                        {
                            f.WindowState = FormWindowState.Normal; f.TopMost = true; f.TopMost = false;
                            return;
                        }
                    }
                }
                else
                {
                    //2 기존 채팅방 없다면 디비에 채팅방 만들고 chat_code넘겨서 새 채팅창 열기
                    ChatDto cdto = new ChatDto();
                    chat_code = cdto.StartChat(LoginInfo.login.id, LoginInfo.selectedUser.id);
                }
                ChatForm cc = new ChatForm(clientSocket, chat_code);
                cc.Tag = chat_code.ToString();
                cc.Show();
                Init();
            }
            catch
            {
                MessageBox.Show("먼저 채팅할 친구를 선택해주세요.");
            }
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
            notifyIcon1.Dispose();
            OnlineUserInformation.Stop();
            clientSocket.Close();
            Close();
        }
        void KeepChatList(object sender, EventArgs e)
        {
            cdto.Load();
            listBox3.DataSource = ChatDto.MyChatList;
            listBox3.DisplayMember = "chat_display";
            listBox3.ValueMember = "chat_users";
        }
        private void listBox3_MouseDoubleClick(object sender, MouseEventArgs e) //채팅목록에서 채팅창열기
        {
            int selectedIndex = -1;
            Point point = e.Location;
            selectedIndex = listBox3.IndexFromPoint(point);

            if (selectedIndex != -1)
            {
                int chat_code = -1;
                if (ChatDto.MyChatList.Exists(x => x.chat_users == UserDto.Users.Find(a => a.id == listBox3.SelectedValue.ToString()).id))
                {
                    chat_code = ChatDto.MyChatList.Find(x => x.chat_users == UserDto.Users.Find(a => a.id == listBox3.SelectedValue.ToString()).id).chat_code;
                }
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == "ChatForm" && f.Tag.ToString() == chat_code.ToString())
                    {
                        f.WindowState = FormWindowState.Normal; f.TopMost = true; f.TopMost = false;
                        return;
                    }
                }
                ChatForm cc = new ChatForm(clientSocket, chat_code);
                cc.Tag = chat_code.ToString();
                cc.Show();
            }
        }
        public  class MyThread
        {
            private Boolean isRun = true;
            Thread t;
            ListBox listBox1;
            ListBox listBox2;
            public void Start(ListBox a, ListBox b)
            {
                isRun = true;
                listBox1 = a;
                listBox2 = b;
                t = new Thread(OnlineUsers);
                t.Start();
            }
            private void OnlineUsers()
            {
                while (isRun)
                {
                    byte[] bytesFrom = new byte[(int)clientSocket.ReceiveBufferSize];
                    serverStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    string dataFromServer = System.Text.Encoding.UTF8.GetString(bytesFrom);
                    string[] data = dataFromServer.Split('$');
                    if (data[0] == "OnlineUsers")
                    {
                        List<string> onlineUsers = new List<string>();
                        FriendDto.Online_Friends = new List<Model.vo.User>();
                        FriendDto.Offline_Friends = new List<Model.vo.User>();
                        foreach (var a in data)
                        {
                            if (a != "OnlineUsers")
                                onlineUsers.Add(a);
                        }
                        foreach (var user in FriendDto.Friends)
                        {
                            if (onlineUsers.Exists(x => x == user.id))
                            {
                                FriendDto.Online_Friends.Add(UserDto.Users.Find(x => x.id == user.id));
                            }
                            else
                            {
                                FriendDto.Offline_Friends.Add(UserDto.Users.Find(x => x.id == user.id));
                            }
                        }

                        listBox1.DataSource = FriendDto.Online_Friends;
                        listBox1.DisplayMember = "name";
                        listBox1.ValueMember = "id";
                        listBox2.DataSource = FriendDto.Offline_Friends;
                        listBox2.DisplayMember = "name";
                        listBox2.ValueMember = "id";
                    }
                }
            }
            public void Stop()
            {
                isRun = false;
                t.Abort();
            }
        }
    }
}
