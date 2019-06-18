using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test0514_ChatClient.Model.vo;

namespace test0514_ChatClient.Model.dto
{
    public static class LoginInfo
    {
        private static User _login;
        private static User _selectedUser;
        private static string saveId_;
        private static string savePw_;
        internal static User login
        {
            get { return _login; }
            set { _login = value; }
        }

        internal static User selectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; }
        }

        public static string saveId
        {
            get { return saveId_; }
            set { saveId_ = value; }
        }
        public static string savePw
        {
            get { return savePw_; }
            set { savePw_ = value; }
        }
    }
}
