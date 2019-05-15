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
     class UserDto
    {
        public static List<User> Users;
        UserDao userDao = new UserDao();

        internal void Join(string id, string pw, string name, string gender)
        {
            User user = new User(id, pw, name, gender);
            userDao.Join(user);
        }

        internal void Login(string id)
        {
            User u = new User();
            u = Users.Find(x => x.id == id);
            LoginInfo.login = u;
        }
        internal  void Load()
        {
            Users = userDao.SelectAll();
        }
        internal void Update(string name, string password, string msg)
        {
            User user = LoginInfo.login;
            user.name = name;
            user.password = password;
            //user.msg = msg;
            userDao.Update(user);
        }
    }
}
