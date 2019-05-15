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
using test0514_ChatClient.Model.vo;

namespace test0514_ChatClient
{
    public partial class FriendDetailForm : Form
    {
        public FriendDetailForm()
        {
            InitializeComponent();
            button1.Visible = false;
            Init();

        }
        private void Init()
        {
            if (FriendDto.Friends.Find(x => x.id == LoginInfo.selectedUser.id)==null &&
               FriendDto.Received_Requests.Find(x => x.id == LoginInfo.selectedUser.id) ==null&&
               LoginInfo.login.id != LoginInfo.selectedUser.id)
            {
                button1.Visible = true;
            }
            label1.Text = LoginInfo.selectedUser.name;
            label2.Text = LoginInfo.selectedUser.name + " / " + LoginInfo.selectedUser.id;
            if (LoginInfo.selectedUser.gender == "남자")
            {
                pictureBox1.Load(@"C:\dev\19-1modern\test0514-ChatClient\test0514-ChatClient\img\default_man.jpg");
            }
            else
            {
                pictureBox1.Load(@"C:\dev\19-1modern\test0514-ChatClient\test0514-ChatClient\img\default_woman.jpg");
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button1_Click(object sender, EventArgs e) //친구요청 버튼
        {
            FriendDto friendDto = new FriendDto();
            friendDto.Load();
            if(FriendDto.Sent_Requests.Find(x => x.id == LoginInfo.selectedUser.id)!= null)
            {
                MessageBox.Show("이미 보낸 친구입니다.");
            }
           else
            {
                friendDto.Request();
                MessageBox.Show(LoginInfo.selectedUser.id+" 님께 요청을 보냈습니다.");
                this.Close();
            }
        }
    }
}
