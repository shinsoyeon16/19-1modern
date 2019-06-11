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
    { //일대일 채팅 기능만듥릴
        string readData = "";
        public static TcpClient clientSocket = new TcpClient();
        public static NetworkStream serverStream = default(NetworkStream);
        Chat chat = new Chat();
        ChatDto cdto = new ChatDto();
        UserDto udto = new UserDto();

        public ChatForm(TcpClient _clientSocket, int chat_code)
        {
            ////로그인 안된 상태면 창 종료
            if (LoginInfo.login == null) this.Close();

            ////채팅방 세팅을 위해 데이터 불러오기
            clientSocket = _clientSocket;
            serverStream = clientSocket.GetStream();
            cdto.Load(); udto.Load();
            //chat = ChatDto.ChatList.Find(x => x.chat_code == chat_code);
            //List<string> chat_users = cdto.ReadChatUser(chat_code);
            //string chat_id = chat_users[0];
            //User user = UserDto.Users.Find(x => x.id == chat_id);


            //// 채팅할 상대유저의 정보를 폼에 띄우기
            ////label1.Text = user.name + "  (" + user.id + ")";
            ////label2.Text = user.message;
            ////MemoryStream ms = new MemoryStream(user.image);
            ////pictureBox1.Image = Image.FromStream(ms);
            ////pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            ///

            readData = "[ sf님과 대화를 시작합니다. ]";
            msg();
            //Thread ctThread = new Thread(getMessage);
            //ctThread.Start();
        }
        void TextBox1_ScrollEvent(object sender, EventArgs e)
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }
        private void button1_Click(object sender, EventArgs e) //메세지 전송버튼
        {
            //byte[] outStream = System.Text.Encoding.ASCII.GetBytes("chat$" + LoginInfo.login.id + "$" + chat.index + "$" + textBox2.Text + "$");
            //serverStream.Write(outStream, 0, outStream.Length);
            //serverStream.Flush();
            //textBox2.Text = "";
        }

        private void getMessage()
        {
            //while (true)
            //{
            //    byte[] inStream = new byte[(int)clientSocket.ReceiveBufferSize];
            //    serverStream.Read(inStream, 0, clientSocket.ReceiveBufferSize);
            //    string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            //    string[] data = returndata.Split('$');
            //    if (data[0] == "chat" && int.Parse(data[2]) == chat.index)
            //        readData = "" + data[3];
            //    msg();
            //}
           
        }

        private void msg()
        {
            textBox1.Text = textBox1.Text + Environment.NewLine + " >> " + readData; 
            MessageBox.Show(textBox1.Text);
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
