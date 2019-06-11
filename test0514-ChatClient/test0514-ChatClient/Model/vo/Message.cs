using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test0514_ChatClient.Model.vo
{
    public class Message
    {
        private string message_;

        public string message
        {
            get { return message_; }
            set { message_ = value; }
        }
        private DateTime time_;

        public DateTime time
        {
            get { return time_; }
            set { time_ = value; }
        }
        private string id_;

        public string id
        {
            get { return id_; }
            set { id_ = value; }
        }

    }
}
