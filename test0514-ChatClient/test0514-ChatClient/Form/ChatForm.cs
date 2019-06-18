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
using System.Windows.Forms;
using test0514_ChatClient.Model.dto;
using test0514_ChatClient.Model.vo;

namespace test0514_ChatClient
{
    public partial class ChatForm : Form
    { 
        List<Model.vo.Message> chatLog = new List<Model.vo.Message>();
        public static TcpClient clientSocket;
        public static NetworkStream serverStream;
        Chat chat = new Chat();
        User chat_user = new User();
        ChatDto cdto = new ChatDto();
        UserDto udto = new UserDto();
        Thread ctThread;
        public ChatForm(TcpClient _clientSocket, int chat_code)
        {
            InitializeComponent(); //  InitailizeComponent 메서드에서 폼 속성 및 자식 컨트롤 배치 등의 작업을 수행하기 때문입니다. okok

            //로그인 안된 상태면 창 종료
            if (LoginInfo.login == null) this.Close();
            clientSocket = _clientSocket;

            //채팅방 세팅을 위해 데이터 불러오기
            serverStream = clientSocket.GetStream();
            cdto.Load(); udto.Load();
            chat = ChatDto.ChatList.Find(x => x.chat_code == chat_code);
            chatLog = cdto.ReadMessage(chat.chat_code);
            chat_user = UserDto.Users.Find(x => x.id == chat.chat_users);
            Init();
            ctThread = new Thread(getMessage);
            ctThread.Start();
        }
        void Init()
        {
            // 채팅할 상대유저의 정보를 폼에 띄우기
            label1.Text = chat_user.name + "  (" + chat_user.id + ")";
            label2.Text = chat_user.message;
            MemoryStream ms = new MemoryStream(chat_user.image);
            pictureBox1.Image = Image.FromStream(ms);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            msg();
        }
        void TextBox1_ScrollEvent(object sender, EventArgs e)
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }
        private void button1_Click(object sender, EventArgs e) //메세지 전송버튼
        {
            textBox2.Text = textBox2.Text.Trim('\r');
            textBox2.Text = textBox2.Text.Trim('\n');
            if (textBox2.Text != "" && textBox2.Text != " ")
            {
                byte[] outStream = System.Text.Encoding.UTF8.GetBytes("chat$" + LoginInfo.login.id + "$" + chat.chat_code + "$" + textBox2.Text + "$");
                serverStream.Write(outStream, 0, outStream.Length);
                serverStream.Flush();
            }
            textBox2.Text = null;
        }

        private void getMessage()
        {
            while (true)
            {
                byte[] inStream = new byte[(int)clientSocket.ReceiveBufferSize];
                serverStream.Read(inStream, 0, clientSocket.ReceiveBufferSize);
                string returndata = System.Text.Encoding.UTF8.GetString(inStream);
                string[] data = returndata.Split('$');
                if (data[0] == "chat" && int.Parse(data[1]) == chat.chat_code)
                {
                    chatLog = cdto.ReadMessage(chat.chat_code);
                }
                msg();
            }
        }

        private void msg()
        {
            string log = "";
            foreach (var a in chatLog)
            {
                if (LoginInfo.login.id == a.id)
                    log = log + Environment.NewLine + "나 : " + a.message + " (" + a.time.ToString("yyyy-MM-dd HH:mm:ss") + ")";
                else
                    log = log + Environment.NewLine + UserDto.Users.Find(x => x.id == a.id).name + " : " + a.message + " (" + a.time.ToString("yyyy-MM-dd HH:mm:ss") + ")";
            }
            textBox1.Text = log;
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                button1_Click(sender, e);
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ctThread.Abort();
        }
    }
}

/*  1대일 건들기 전 버전
    public partial class ChatForm : Form
{
    TcpClient clientSocket = new TcpClient();
    NetworkStream serverStream = default(NetworkStream);
    string readData = null;

    public ChatForm()
    {
        InitializeComponent();

        //로그인 안된 상태면 창 종료
        if (LoginInfo.login == null) this.Close();

        //서버 접속
        this.FormClosing += Form_Closing;
        clientSocket.Connect("127.0.0.1", 8888);
        byte[] outStream = System.Text.Encoding.ASCII.GetBytes(LoginInfo.login.id + "$");
        serverStream = clientSocket.GetStream();
        serverStream.Write(outStream, 0, outStream.Length);
        readData = "[ Hello " + LoginInfo.login.id + ", You Can Start Chatting Now ]";
        msg();


        Thread ctThread = new Thread(getMessage);
        ctThread.Start();
    }
    void TextBox1_ScrollEvent(object sender, EventArgs e)
    {
        textBox1.SelectionStart = textBox1.Text.Length;
        textBox1.ScrollToCaret();
    }
    private void button1_Click(object sender, EventArgs e) //메세지 전송버튼
    {
        byte[] outStream = System.Text.Encoding.ASCII.GetBytes(LoginInfo.login.id + "$" + textBox2.Text + "$");
        serverStream.Write(outStream, 0, outStream.Length);
        serverStream.Flush();
        textBox2.Text = "";
    }

    private void getMessage()
    {
        while (true)
        {
            int buffSize = 0;
            byte[] inStream = new byte[(int)clientSocket.ReceiveBufferSize];
            buffSize = clientSocket.ReceiveBufferSize;
            serverStream.Read(inStream, 0, buffSize);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            readData = "" + returndata;
            msg();
        }
    }

    private void msg()
    {
        if (this.InvokeRequired)
            this.Invoke(new MethodInvoker(msg));
        else
            textBox1.Text = textBox1.Text + Environment.NewLine + " >> " + readData;
    }

    private void Form_Closing(object sender, FormClosingEventArgs e)
    {
        serverStream = clientSocket.GetStream();
        byte[] outStream = System.Text.Encoding.ASCII.GetBytes(LoginInfo.login.id + "$" + "exit" + "$");
        serverStream.Write(outStream, 0, outStream.Length);
    }


}
 */
