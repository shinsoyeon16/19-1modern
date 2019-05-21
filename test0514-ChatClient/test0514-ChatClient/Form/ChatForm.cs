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

namespace test0514_ChatClient
{
    //1. 일대일 채팅만들기 2. 일대다 채팅  3. 전체체팅 연결
    public partial class ChatForm : Form
    {
        string readData = null;

        public ChatForm()
        {
            InitializeComponent();

            //로그인 안된 상태면 창 종료
            if (LoginInfo.login == null) this.Close();

            // 채팅할 상대유저의 정보를 폼에 띄우기
            label1.Text = LoginInfo.selectedUser.name + "  (" + LoginInfo.selectedUser.id + ")";
            label2.Text = LoginInfo.selectedUser.message;
            MemoryStream ms = new MemoryStream(LoginInfo.selectedUser.image);
            pictureBox1.Image = Image.FromStream(ms);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            //서버 접속 - 서버에 로그인아이디와 채팅아이디를 보냄
            this.FormClosing += Form_Closing;
            
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(LoginInfo.login.id + "$"+LoginInfo.selectedUser.id+"$");
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
