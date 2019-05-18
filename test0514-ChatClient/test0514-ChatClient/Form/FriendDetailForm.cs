using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        FriendDto friendDto = new FriendDto();
        public FriendDetailForm()
        {
            InitializeComponent();
            button1.Visible = false;
            Init();

        }
        private void Init()
        {
            // DTO 객체에 db정보 초기화
            UserDto userDto = new UserDto();
            FriendDto dto = new FriendDto();
            userDto.Load();
            dto.Load();

            if (FriendDto.Friends.Find(x => x.id == LoginInfo.selectedUser.id)==null &&
               FriendDto.Received_Requests.Find(x => x.id == LoginInfo.selectedUser.id) ==null&&
               LoginInfo.login.id != LoginInfo.selectedUser.id)
            {
                button1.Visible = true;
            }
            // 로그인된 유저의 정보를 폼에 띄우기
            label1.Text = LoginInfo.selectedUser.name + "  (" + LoginInfo.selectedUser.id + ")";
            label2.Text = LoginInfo.selectedUser.message;
            MemoryStream ms = new MemoryStream(LoginInfo.selectedUser.image);
            pictureBox1.Image = Image.FromStream(ms);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            this.OnLoad(new EventArgs());
        }

        private void button1_Click(object sender, EventArgs e) //친구요청 버튼
        {
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
