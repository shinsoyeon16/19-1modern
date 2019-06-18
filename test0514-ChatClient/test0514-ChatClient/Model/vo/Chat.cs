using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test0514_ChatClient.Model.vo
{
    public class Chat
    { 
        private int chat_code_;
        private string  name_;
        private string last_message_;
        private DateTime last_time_;
        private string chat_users_="";
        private string chat_display_;

        public string chat_display
        {
            get { return chat_display_; }
            set { chat_display_ = value; }
        }

        public string chat_users
        {
            get { return chat_users_; }
            set { chat_users_ = value; }
        }


        public int chat_code
        {
            get { return chat_code_; }
            set { chat_code_ = value; }
        }

        public string  name
        {
            get { return name_; }
            set { name_ = value; }
        }

        public string last_message
        {
            get { return last_message_; }
            set { last_message_ = value; }
        }

        public DateTime last_time
        {
            get { return last_time_; }
            set { last_time_ = value; }
        }

        public Chat()
        {

        }
    }
}
