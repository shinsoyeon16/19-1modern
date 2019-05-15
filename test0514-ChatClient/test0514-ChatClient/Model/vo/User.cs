namespace test0514_ChatClient.Model.vo
{
     class User
    {
        private string _id;
        private string  _password;
        private string _name;
        private string _gender;

        public string id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string  password
        {
            get { return _password; }
            set { _password = value; }
        }
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string gender
        {
            get { return _gender; }
            set { _gender = value; }
        }

        public User(string _id, string _password, string _name, string _gender)
        {
            id = _id;
            password = _password;
            name = _name;
            gender = _gender;
        }

        public User()
        {
        }
    }
}
