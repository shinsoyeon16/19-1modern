//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace test0514_ChatServerApp
//{
//    public partial class Form2 : Form
//    {
//    public static Hashtable clientsList = new Hashtable();
//        public Form2()
//        {
//            InitializeComponent();
//            console("[ 채팅 프로그램 시작 ]");
//        }

//        public void console(string msg)
//        {
//            textBox1.Text = textBox1.Text + Environment.NewLine + msg;
//        }

//        public static string[] ReadFromClient(TcpClient clientSocket)
//        {
//            NetworkStream networkStream = clientSocket.GetStream();
//            byte[] bytesFrom = new byte[(int)clientSocket.ReceiveBufferSize];
//            networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
//            string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
//            string[] data = dataFromClient.Split('$');
//            return data;
//        }
//        public static void broadcast(string msg, string id, bool flag)
//        {
//            foreach (DictionaryEntry Item in clientsList)
//            {
//                TcpClient broadcastSocket;
//                broadcastSocket = (TcpClient)Item.Value;
//                NetworkStream broadcastStream = broadcastSocket.GetStream();
//                Byte[] broadcastBytes = null;

//                if (flag == true && id != Item.Key.ToString())
//                    broadcastBytes = Encoding.ASCII.GetBytes(id + " : " + msg);
//                else
//                    broadcastBytes = Encoding.ASCII.GetBytes(msg);
//                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
//                broadcastStream.Flush();
//            }
//        }

//        private void Form1_Load(object sender, EventArgs e)
//        {
//            TcpListener serverSocket = new TcpListener(8881);
//            TcpClient clientSocket = default(TcpClient);
//            serverSocket.Start();
//            console("가동완료");
//            while ((true))
//            {
//                // if (this.close()) break; //서버프로그램 종료기능
//                clientSocket = serverSocket.AcceptTcpClient(); //접속한클라이언트를 클라소켓에 대입
//                string[] data = ReadFromClient(clientSocket);
//                if (data[0] == "login")
//                {
//                    //broadcast("[ " + data[0] + " Start Chatting ]", data[0], false);

//                    clientsList.Add(data[1], clientSocket);


//                    textBox1.Text = textBox1.Text + "\n[ " + data[1] + " 유저 로그인 ]";
//                    handleClient client = new handleClient();
//                    client.LogonClient(data[1], clientSocket);
//                }
//            }
//            serverSocket.Stop();
//        }
//    } //end Form1 class
//}

