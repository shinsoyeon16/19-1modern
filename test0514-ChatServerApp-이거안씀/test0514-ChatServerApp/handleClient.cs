//using System;
//using System.Net.Sockets;
//using System.Threading;

//namespace test0514_ChatServerApp
//{
//    public partial class Form2
//    {
//        public class handleClient
//        {
//            TcpClient clientSocket;
//            string id;
//            public void LogonClient(string id, TcpClient inClientSocket)
//            {
//                this.id = id;
//                this.clientSocket = inClientSocket;
//                Thread state = new Thread(State);
//                state.Start();
//            }
//            private void State()
//            {
//                string[] data = ReadFromClient(clientSocket);
//                if (data[0] == "chatStart")
//                {
//                    Thread ctThread = new Thread(doChat);
//                    ctThread.Start();
//                    //if()이미 목록에있는지없는지 확인
//                }
//                else if (data[0] == "chat")
//                {
//                    //if(data[2].)
//                }
//                else if (data[0] == "logout")
//                {
//                    LogoutClient();
//                }
//            }
//            public void LogoutClient()
//            {
//                clientsList.Remove(this.id);
//                //console("[ " + this.id + " 유저 채팅 종료 ]");
//                clientSocket.Close();
//            }
//            private void doChat()
//            {
//                byte[] bytesFrom = new byte[(int)clientSocket.ReceiveBufferSize];
//                string dataFromClient = null;
//                Byte[] sendBytes = null;
//                string serverResponse = null;

//                while ((true))
//                {
//                    try
//                    {
//                        NetworkStream networkStream = clientSocket.GetStream();
//                        networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
//                        dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
//                        string[] data = dataFromClient.Split('$'); //data[0]은 보낸아이디, data[1]은 데이터
//                        if (data[1] != "exit")
//                        {
//                            Console.WriteLine("- " + id + " : " + data[1]);
//                            broadcast(data[1], data[0], true);
//                        }
//                        else break;
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine(ex.ToString());
//                    }
//                }//end while
//            }//end doChat
//        }
//    }
//} //end namespace