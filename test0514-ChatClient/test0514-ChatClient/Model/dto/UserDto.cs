using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            User user = new User(id, pw, name, gender,"", DefaultProfile(gender));
            userDao.Join(user);
        }
        private byte[] DefaultProfile(string gender)
        {
            string defaultImg = @"..\..\img\default_";
            if (gender == "남자") defaultImg = defaultImg + "man.jpg";
            else defaultImg += "woman.jpg";
            byte[] ImageData;
            FileStream fs = new FileStream(defaultImg, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            ImageData = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            return ImageData;
        }
        internal void Login(string id)
        {
            User u = new User();
            u = Users.Find(x => x.id == id);
            LoginInfo.login = u;
        }
        internal void Load()
        {
            Users = userDao.SelectAll();
        }
        internal void Update(string name, string password, string msg)
        {
            User user = LoginInfo.login;
            user.name = name;
            user.password = password;
            user.message= msg;
            userDao.Update(user);
            Load();  Login(LoginInfo.login.id);
        }
        internal void UpdatePicture(string filename)
        {
                // 사진파일로부터 byte[]를 추출한다.
                byte[] ImageData;
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                ImageData = br.ReadBytes((int)fs.Length);
                br.Close(); fs.Close();
                userDao.UpdatePicture(ImageData, LoginInfo.login.id);
                Load(); Login(LoginInfo.login.id);
        }
        internal void ResetPicture()
        {
            userDao.UpdatePicture(DefaultProfile(LoginInfo.login.gender), LoginInfo.login.id);
            Load(); Login(LoginInfo.login.id);
        }
    }
}
