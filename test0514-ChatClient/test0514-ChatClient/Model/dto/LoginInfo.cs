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

        internal static User login
        {
            get { return _login; }
            set { _login = value; }
        }
        private static User _selectedUser;

        internal static User selectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; }
        }

    }
}
