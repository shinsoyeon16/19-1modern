using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test0514_ChatClient.Model.vo;

namespace test0514_ChatClient.Model.dao
{
    class UserDao
    {

        MySqlConnection conn = new MySqlConnection("Server=localhost;Database=chat;Uid=root;Pwd=1234;");

        internal void Join(User user)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO user (id, password, name, gender, image) VALUES ('" + user.id + "', '" + user.password + "','" + user.name + "', '" + user.gender + "', @img)", conn);
            cmd.Parameters.Add("@img", MySqlDbType.Blob);
            cmd.Parameters["@img"].Value = user.image;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        internal List<User> SelectAll()
        {
            List<User> list = new List<User>();
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM user", conn);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "user");
            conn.Close();

            foreach (DataRow r in ds.Tables[0].Rows)
            {
                byte[] img = (byte[])r[4];
                list.Add(new User(r[0].ToString(), r[1].ToString(), r[2].ToString(), r[3].ToString(), r[5].ToString(), img));
            }
            da.Dispose();
            return list;
        }
        internal void Update(User user)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Update user set name = '" + user.name + "', password = '" + user.password + "', message = '" + user.message + "'  where id = '" + user.id + "'", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        internal void UpdatePicture(byte[] ImageData, string id)
        {
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("Update user set image = @img where id ='"+id+"'", conn);
            cmd.Parameters.Add("@img", MySqlDbType.Blob);
            cmd.Parameters["@img"].Value = ImageData;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
