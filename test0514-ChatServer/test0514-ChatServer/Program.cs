using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
