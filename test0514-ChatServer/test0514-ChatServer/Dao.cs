using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test0514_ChatServer
{
   public class Dao
    {
        MySqlConnection conn = new MySqlConnection("Server=localhost;Database=chat;Uid=root;Pwd=1234;");

        public void SaveMessage(string[] data)
        {
                conn.Open();
                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                MySqlCommand cmd = new MySqlCommand("INSERT INTO message (chat_code, id, time, message) VALUES('" + data[2] + "','" + data[1] + "', '" + time + "', '" + data[3] + "')", conn);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand("update chat set last_message='" + data[3] + "', last_time='" + time + "' where chat_code='" + data[2] + "'", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
        }

        public List<string> LoadChatUsers(string[] data)
        {
            List<string> list = new List<string>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT id from chat_users where chat_code='" + data[2] + "'", conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(rdr[0].ToString());
            }
            rdr.Close();
            conn.Close();
            return list;
        }
    }
}
