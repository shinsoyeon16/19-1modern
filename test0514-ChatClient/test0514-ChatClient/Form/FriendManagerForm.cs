using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using test0514_ChatClient.Model.dto;

namespace test0514_ChatClient
{
    public partial class FriendManagerForm : Form
    {
        UserDto userDto = new UserDto();
        FriendDto friendDto = new FriendDto();
        public FriendManagerForm()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            userDto.Load(); friendDto.Load();
            this.OnLoad(new EventArgs());
        }
        private void ListBinding(object sender, EventArgs e)
        {
            listBox1.DataSource = FriendDto.Received_Requests;
            listBox1.DisplayMember = "name";
            listBox1.ValueMember = "id";
            listBox2.DataSource = FriendDto.Friends;
            listBox2.DisplayMember = "name";
            listBox2.ValueMember = "id";
        }
        private void button1_Click(object sender, EventArgs e) //친구 추가란 아이디 검색버튼
        {
            string id = textBox1.Text;
            if (id == "")
            {
                MessageBox.Show("검색할 수 없습니다."); return;
            }
            if( UserDto.Users.Find(x => x.id == id) == null)
            {
                MessageBox.Show("검색 정보가 없습니다.");
            }
            else
            {
                LoginInfo.selectedUser = UserDto.Users.Find(x => x.id == id);
                if (new FriendDetailForm().ShowDialog() == DialogResult.Cancel) Init();
            }
            textBox1.Text = "";
        }
        private void button7_Click(object sender, EventArgs e) //친구 추가란 이름 검색버튼
        {
            string name = textBox2.Text;
            if (name == "")
            {
                MessageBox.Show("검색할 수 없습니다."); return;
            }
            if (UserDto.Users.Find(x => x.name == name) == null)
            {
                MessageBox.Show("검색 정보가 없습니다.");
            }
            else
            {
                LoginInfo.selectedUser = UserDto.Users.Find(x => x.name == name);
                if (new FriendDetailForm().ShowDialog() == DialogResult.Cancel) Init();
            }
            textBox2.Text = "";
        }

        //받은 요청
        private void button4_Click(object sender, EventArgs e) //친구요청란 상세보기 버튼
        {
            LoginInfo.selectedUser = UserDto.Users.Find(x => x.id == listBox1.SelectedValue.ToString());
            if (new FriendDetailForm().ShowDialog() == DialogResult.Cancel) Init();
        }
        private void button2_Click(object sender, EventArgs e) //친구요청란 수락 버튼
        {
            friendDto.Response(LoginInfo.login.id, listBox1.SelectedValue.ToString());
            Init();
        }
        private void button3_Click(object sender, EventArgs e) //친구요청란 거절 버튼
        {
            friendDto.Reject(LoginInfo.login.id, listBox1.SelectedValue.ToString());
            Init();
        }

        //친구관리
        private void button5_Click(object sender, EventArgs e) //친구관리란 상세보기 버튼
        {
            LoginInfo.selectedUser = UserDto.Users.Find(x => x.id == listBox2.SelectedValue.ToString());
            if (new FriendDetailForm().ShowDialog() == DialogResult.Cancel) Init();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            friendDto.Breakup(LoginInfo.login.id, listBox2.SelectedValue.ToString());
            Init();
        }
    }
}
