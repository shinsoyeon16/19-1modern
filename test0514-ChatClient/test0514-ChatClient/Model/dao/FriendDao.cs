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

        private List<User> MatchingUser(List<string> friends)
        {
            List<User> list = new List<User>();
            conn.Open();
            //친구목록 리스트에 맞는 유저정보 가져오기
            foreach (string friend_id in friends)
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM user where id = '" + friend_id + "'", conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new User(rdr[0].ToString(), rdr[1].ToString(), rdr[2].ToString(), rdr[3].ToString()));
                }
                rdr.Close();
            }
            conn.Close();
            return list;
        }

        internal List<User> SelectAll(string id)
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
            return MatchingUser(friends);
        }

        internal List<User> Received(string id)
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
            return MatchingUser(friends);
        }

        internal List<User> Sent(string id)
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
            return MatchingUser(friends);
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

