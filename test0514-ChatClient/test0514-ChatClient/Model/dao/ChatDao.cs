using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test0514_ChatClient.Model.vo;

namespace test0514_ChatClient.Model.dao
{
    public class ChatDao
    {
        MySqlConnection conn = new MySqlConnection("Server=localhost;Database=chat;Uid=root;Pwd=1234;");

        public List<Chat> SelectChat()
        {
            List<Chat> list = new List<Chat>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * from chat", conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Chat chat = new Chat();
                chat.chat_code = int.Parse(rdr[0].ToString());
                chat.name = rdr[1].ToString();
                chat.last_message = rdr[2].ToString();
                chat.last_time = DateTime.Parse(rdr[3].ToString());
                list.Add(chat);
            }
            rdr.Close();
            conn.Close();
            return list;
        }
        public string SelectChatUsers(string login_id, int chat_code)
        {
            conn.Open();
            MySqlCommand cmd;
            MySqlDataReader rdr;
            string result = "";
            cmd = new MySqlCommand("SELECT * from chat_users where chat_code='" + chat_code + "'", conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr[1].ToString() != login_id) result = result + rdr[1].ToString();
            }
            rdr.Close();
            conn.Close();
            return result;
        }
        public List<int> SelectMyChat(string id)
        {
            List<int> list = new List<int>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT chat_code from chat_users where id = '" + id + "'", conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(int.Parse(rdr[0].ToString()));
            }
            rdr.Close();
            conn.Close();
            return list;
        }
        public int CreateChat(string id, string chat_user) //새로운 채팅방 만들기
        {
            conn.Open();
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            MySqlCommand cmd = new MySqlCommand("INSERT INTO chat(last_message, last_time) VALUES('  ','" + time + "')", conn);
            cmd.ExecuteNonQuery();
            int chat_code = SelectChatCode(time); //자동생성된 채팅방번호를 받아온다

            //채팅방의 pk를 사용하여 채팅유저목록 삽입
            InsertChatUser(chat_code, id); InsertChatUser(chat_code, chat_user);
            conn.Close();
            return chat_code;
        }
        public int SelectChatCode(string time) //새로 만들어진 채팅방의 pk 받아오기
        {
            MySqlCommand cmd = new MySqlCommand("select chat_code from chat where last_time='" + time + "'", conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            int result = 0;
            while (rdr.Read()) result = int.Parse(rdr[0].ToString());
            rdr.Close();
            return result;
        }
        public void InsertChatUser(int chat_code, string id)
        {

            MySqlCommand cmd = new MySqlCommand("INSERT INTO chat_users (chat_code, id) VALUES('" + chat_code + "','" + id + "')", conn);
            cmd.ExecuteNonQuery();
        }
        public List<string> SelectChatUser(int chat_code)
        {
            List<string> list = new List<string>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT id from chat_users where chat_code = '" + chat_code + "'", conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(rdr[0].ToString());
            }
            rdr.Close();
            conn.Close();
            return list;
        }
        public List<vo.Message> SelectMessages(int chat_code)
        {
            List<vo.Message> list = new List<vo.Message>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * from message where chat_code='"+chat_code+"'", conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                vo.Message message = new vo.Message();
                message.id = rdr[1].ToString();
                message.time = DateTime.Parse( rdr[2].ToString());
                message.message = rdr[3].ToString();
                list.Add(message);
            }
            rdr.Close();
            conn.Close();
            return list;
        }
    }
}
