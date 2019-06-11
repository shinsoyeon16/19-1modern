using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace test0514_ChatServer
{
    class Program
    {
        public static Hashtable clientsList = new Hashtable();
        public static int chat_index = 0;
        static void Main(string[] args)
        {
            TcpListener serverSocket = new TcpListener(8888);
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            Console.WriteLine("[ 채팅 프로그램 시작 ]");
            while ((true))
            {
                Console.WriteLine("다시 와일문돈당");
                // if (this.close()) break; //서버프로그램 종료기능
                clientSocket = serverSocket.AcceptTcpClient(); //접속한클라이언트를 클라소켓에 대입
                Console.WriteLine("다시 소켓받는당");
                string[] data = ReadFromClient(clientSocket);
                if (data[0] == "login")
                {
                    //broadcast("[ " + data[0] + " Start Chatting ]", data[0], false);

                    clientsList.Add(data[1], clientSocket);

                    Console.WriteLine("[ " + data[1] + " 유저 로그인 ]");
                    handleLogin client = new handleLogin();
                    client.Login(data[1], clientSocket);
                }

            }
            serverSocket.Stop();
            Console.WriteLine("exit");
            Console.ReadLine();
        }
        public static string[] ReadFromClient(TcpClient clientSocket)
        {
            NetworkStream networkStream = clientSocket.GetStream();
            byte[] bytesFrom = new byte[(int)clientSocket.ReceiveBufferSize];
            networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
            string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
            string[] data = dataFromClient.Split('$');
            return data;
        }

        public static void Chatbroadcast(string msg, string id, int index)
        {
            // List<string> chat_users = ChatList.Find(x => x.index == index).user;
            foreach (DictionaryEntry Item in clientsList)
            {
                //if (chat_users.Contains(Item.Value))
                //{
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;
                if (id != Item.Key.ToString())
                    broadcastBytes = Encoding.ASCII.GetBytes(id + " : " + msg);
                else
                    broadcastBytes = Encoding.ASCII.GetBytes(msg);
                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
                // }
            }
        }  //end broadcast function


        public class handleLogin
        {
            TcpClient clientSocket;
            string id;
            private Boolean isRun = true;
            public void Login(string id, TcpClient inClientSocket)
            {
                this.id = id;
                this.clientSocket = inClientSocket;
                while (isRun)
                {
                    string[] data = ReadFromClient(clientSocket);
                    if (data[0] == "chat")
                    {
                        Console.WriteLine("-- " + data[2] + "번 채팅방 >> " + id + " : " + data[3]);
                        Chatbroadcast(data[3], data[1], int.Parse(data[2]));
                    }
                    //if()이미 목록에있는지없는지 확인
                    else if (data[0] == "logout")
                    {
                        Logout();
                    }
                }
               // handleChat state = new handleChat();
                //state.Run();
            }

            public void Logout()
            {
                isRun = false;
                clientsList.Remove(this.id);
                Console.WriteLine("[ " + this.id + " 유저 채팅 종료 ]");
            }
        } //end class handleClinet
    }
    

    //public class handleChat
    //{
    //        TcpClient clientSocket;
    //        string id;
    //        private Boolean isRun = true;
    //    public void SetState(Boolean state)
    //    {
    //        this.isRun = state;
    //    }
    //    private void State()
    //    {
            
    //    }
    //    public void Run()
    //    {
    //        while (isRun)
    //        {
    //            try
    //            {
    //                State();
    //                Thread.Sleep(100);
    //            }
    //            catch (Exception e) { }
    //        }
    //        Console.WriteLine("스레드 종료");
    //    }
    //}
    //    }
}//end 넴스페슈



/*  여긴 건들기전
 namespace test0514_ChatServer
{
    class Program
    {
        public static Hashtable clientsList = new Hashtable();

        static void Main(string[] args)
        {
            TcpListener serverSocket = new TcpListener(8888);
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            Console.WriteLine("[ 채팅 프로그램 시작 ]");
            while ((true))
            {
                clientSocket = serverSocket.AcceptTcpClient();
                //아이디 얻는 과정
                byte[] bytesFrom = new byte[(int)clientSocket.ReceiveBufferSize];
                string idFromClient = null;
                NetworkStream networkStream = clientSocket.GetStream();
                networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                idFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                idFromClient = idFromClient.Substring(0, idFromClient.IndexOf("$"));
                broadcast("[ "+idFromClient + " Start Chatting ]", idFromClient, false);
                clientsList.Add(idFromClient, clientSocket);


                Console.WriteLine("[ "+idFromClient + " 유저 채팅 시작 ]");
                handleClinet client = new handleClinet();
                client.startClient(clientSocket, idFromClient);
            }
        }

        public static void broadcast(string msg, string id, bool flag)
        {
            foreach (DictionaryEntry Item in clientsList)
            {
                TcpClient broadcastSocket;
                broadcastSocket = (TcpClient)Item.Value;
                NetworkStream broadcastStream = broadcastSocket.GetStream();
                Byte[] broadcastBytes = null;

                if (flag == true && id != Item.Key.ToString())
                    broadcastBytes = Encoding.ASCII.GetBytes(id + " : " + msg);
                else
                    broadcastBytes = Encoding.ASCII.GetBytes(msg);
                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                broadcastStream.Flush();
            }
        }  //end broadcast function
    }//end Main class


    public class handleClinet
    {
        TcpClient clientSocket;
        string id;

        public void startClient(TcpClient inClientSocket, string id)
        {
            this.clientSocket = inClientSocket;
            this.id = id;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }
        public void stopClient()
        {
            Program.clientsList.Remove(this.id);
            Console.WriteLine("[ " + this.id + " 유저 채팅 종료 ]");
            Program.broadcast("[ " + this.id + " Disconnected]", this.id , false);
            clientSocket.Close();
        }
        private void doChat()
        {
            byte[] bytesFrom = new byte[(int)clientSocket.ReceiveBufferSize];
            string dataFromClient = null;
            Byte[] sendBytes = null;
            string serverResponse = null;

            while ((true))
            {
                try
                {
                    NetworkStream networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    string[] data = dataFromClient.Split('$'); //data[0]은 보낸아이디, data[1]은 데이터
                    if (data[1] != "exit")
                    {
                        Console.WriteLine("- " + id + " : " + data[1]);
                        Program.broadcast(data[1], data[0], true);
                    }
                    else break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }//end while
            stopClient();
        }//end doChat
    } //end class handleClinet
}//end 넴스페슈
     */
