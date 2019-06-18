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
            foreach(Chat c in ChatList)
            {
                c.chat_users=cdao.SelectChatUsers(LoginInfo.login.id, c.chat_code);
            }
            MyChatList = new List<Chat>();
            foreach (int a in cdao.SelectMyChat(LoginInfo.login.id))
            {
                MyChatList.Add(ChatList.Find(x => x.chat_code == a));
                MyChatList.Find(x => x.chat_code == a).chat_display = MyChatList.Find(x => x.chat_code == a).name + " / " + MyChatList.Find(x => x.chat_code == a).chat_users + " / " + MyChatList.Find(x => x.chat_code == a).last_message + " (" + MyChatList.Find(x => x.chat_code == a).last_time + ")";
            }
        }
        public int StartChat(string id, string chat_user)
        {
            return cdao.CreateChat(id, chat_user);
        }
        public List<vo.Message> ReadMessage(int chat_code)
        {
            return cdao.SelectMessages(chat_code);
        }
    }
}
