using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test0514_ChatClient.Model.dao;
using test0514_ChatClient.Model.vo;

namespace test0514_ChatClient.Model.dto
{
    public class ChatDto
    {
        public static List<Chat> ChatList;
        public static List<Chat> MyChatList;
        ChatDao cdao = new ChatDao();

        public void Load()
        {
           ChatList = cdao.SelectChat();
            MyChatList = new List<Chat>();
            foreach(int a in cdao.SelectMyChat(LoginInfo.login.id))
            {
            MyChatList.Add(ChatList.Find(x => x.chat_code == a));
            }
        }
        public List<string> ReadChatUser(int chat_code)
        {
           List<string> list = cdao.SelectChatUser(chat_code);
            list.Remove(LoginInfo.login.id);
            return list;
        }
        public int StartChat(string id, string chat_user)
        {
            return cdao.CreateChat(id, chat_user);
        }
        public void SaveMessage()
        {

        }
    }
}
