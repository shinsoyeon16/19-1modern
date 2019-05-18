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
    class FriendDao
    {
        MySqlConnection conn = new MySqlConnection("Server=localhost;Database=chat;Uid=root;Pwd=1234;");
        
        internal List<string> SelectAll(string id)
        {
            List<string> friends = new List<string>();
            conn.Open();
            //친구 목록 리스트 가져오기
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM friend where (id = '" + id + "' or friend_id = '" + id + "' )and isResponsed = 'yes'", conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                if (rdr[0].ToString() == id)
                    friends.Add(rdr[1].ToString());
                else friends.Add(rdr[0].ToString());
            }
            rdr.Close();
            conn.Close();
            return friends;
        }

        internal List<string> Received(string id)
        {
            List<string> friends = new List<string>();
            conn.Open();
            //친구 목록 리스트 가져오기
            MySqlCommand cmd = new MySqlCommand("SELECT id FROM friend where friend_id = '" + id + "' and isResponsed = 'no'", conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                friends.Add(rdr[0].ToString());
            }
            rdr.Close();
            conn.Close();
            return friends;
        }

        internal List<string> Sent(string id)
        {
            List<string> friends = new List<string>();
            conn.Open();
            //친구 목록 리스트 가져오기
            MySqlCommand cmd = new MySqlCommand("SELECT friend_id FROM friend where id = '" + id + "' and isResponsed = 'no'", conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                friends.Add(rdr[0].ToString());
            }
            rdr.Close();
            conn.Close();
            return friends;
        }

        internal void Request(string id, string friend_id)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO friend(id, friend_id) VALUES('" + id + "','" + friend_id + "')", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        internal void Response(string id, string friend_id)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("UPDATE friend SET isResponsed = 'yes' WHERE (id = '" + friend_id + "') and (friend_id = '" + id + "')", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        internal void Reject(string id, string friend_id)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Delete from friend WHERE (id = '" + friend_id + "') and (friend_id = '" + id + "')", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        internal void Breakup(string id, string friend_id)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Delete from friend WHERE (id = '" + friend_id + "' and friend_id = '" + id + "') or (id = '" + id + "' and friend_id = '" + friend_id + "')", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}

