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
        public static List<User> Online_Friends;
        public static List<User> Offline_Friends;
        FriendDao friendDao = new FriendDao();
        UserDao userDao = new UserDao();

        private List<User> MatchingUser(List<string> friends)
        {
            List<User> list = new List<User>();
            //친구목록 리스트에 맞는 유저정보 가져오기
            foreach (string friend_id in friends)
            {
                list.Add(UserDto.Users.Find(x => x.id == friend_id));
            }
            return list;
        }

        internal void Load()
        {
            Friends = MatchingUser(friendDao.SelectAll(LoginInfo.login.id));
            Sent_Requests = MatchingUser(friendDao.Sent(LoginInfo.login.id));
            Received_Requests = MatchingUser(friendDao.Received(LoginInfo.login.id));
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
