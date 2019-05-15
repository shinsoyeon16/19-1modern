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
    public partial class JoinForm : Form
    {
        public JoinForm()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //유효성 검사
            if (textBox1.Text == null || textBox2.Text == null || textBox3.Text == null || textBox4.Text == null || !(radioButton1.Checked || radioButton2.Checked))
            {
                MessageBox.Show("모든 항목을 입력하세요.");
            }
            else if (textBox2.Text != textBox3.Text)  // 비밀번호 확인을 잘못 입력한 경우
            {
                MessageBox.Show("비밀번호와 비밀번호 확인을 정확히 입력하세요.");
            }
            else //모든값이 유효한 경우 중복검사
            {
                string id = textBox1.Text;
                string password = textBox2.Text;
                string name = textBox4.Text;
                string gender = radioButton1.Checked == true ? "남자" : "여자";
                UserDto dto = new UserDto();
                dto.Load(); //나중에 userdto 생성자만들어서 그안에 넣기

                if (UserDto.Users.Find(x => x.id == id) == null)
                {
                    dto.Join(id, password, name, gender);
                    MessageBox.Show("가입 성공");
                    this.Close();
                }
                else MessageBox.Show("중복된 아이디가 있습니다");
            }
        }
    }
}
