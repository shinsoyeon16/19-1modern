using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test0514_ChatClient.Model.dao;
using test0514_ChatClient.Model.vo;

namespace test0514_ChatClient.Model.dto
{
    class FriendDto
    {
        public static List<User> Friends;
        public static List<User> Sent_Requests; //받은 친구요청
        public static List<User> Received_Requests; //보낸 친구요청
        FriendDao friendDao = new FriendDao();
        UserDao userDao = new UserDao();

        internal void Load()
        {
            Friends = friendDao.SelectAll(LoginInfo.login.id);
            Sent_Requests = friendDao.Sent(LoginInfo.login.id);
            Received_Requests = friendDao.Received(LoginInfo.login.id);
        }
        internal void Request()
        {
            friendDao.Request(LoginInfo.login.id, LoginInfo.selectedUser.id);
        }

        internal void Response(string id, string friend_id)
        {
            friendDao.Response(id, friend_id);
        }

        internal void Reject(string id, string friend_id)
        {
            friendDao.Reject(id, friend_id);
        }
        internal void Breakup(string id, string friend_id)
        {
            friendDao.Breakup(id, friend_id);
        }
    }
}
