using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test0514_ChatClient.Model.vo;

namespace test0514_ChatClient.Model.dao
{
     class UserDao
    {

        MySqlConnection conn = new MySqlConnection("Server=localhost;Database=chat;Uid=root;Pwd=1234;");

        internal void Join(User user)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO user VALUES ('"+user.id+"', '"+user.password+ "','" + user.name + "', '" + user.gender + "')", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        internal List<User> SelectAll()
        {
            List<User> list = new  List<User>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM user", conn);
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                list.Add(new User(rdr[0].ToString(), rdr[1].ToString(), rdr[2].ToString(), rdr[3].ToString()));
            }
            rdr.Close();
            conn.Close();
            return list;
        }

        internal void Update(User user)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Update user set name = '"+user.name+"', password = '"+user.password+"' where id = '" + user.id + "'", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
