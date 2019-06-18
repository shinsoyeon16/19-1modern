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
    public partial class LoginForm : Form
    {
        
        public LoginForm()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //유효성 검사
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("모든 항목을 입력하세요.");
            }
            else //모든값이 유효한 경우 중복검사
            {
                string id = textBox1.Text;
                string password = textBox2.Text;
                UserDto dto = new UserDto();
                string dbPassword;
                dto.Load(); //나중에 userdto 생성자만들어서 그안에 넣기
                if (UserDto.Users.Exists(x => x.id == id))
                {
                    dbPassword = UserDto.Users.Find(x => x.id == id).password;
                    if (dbPassword == password)//로그인성공인 경우
                    {
                        if (checkBox1.Checked)
                        {
                            LoginInfo.saveId = textBox1.Text;
                        }
                        else LoginInfo.saveId = "";
                        if (checkBox2.Checked)
                        {
                            LoginInfo.savePw = textBox2.Text;
                        }
                        else LoginInfo.savePw = "";
                        textBox1.Text = LoginInfo.saveId; textBox2.Text = LoginInfo.savePw;
                        this.Visible = false;
                        dto.Login(id);
                        try
                        {
                            if (new IndexForm().ShowDialog() == DialogResult.Cancel) {this.Visible = true; }
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show("접속불가! 서버 관리자에게 문의하세요. \n오류 : " + err);
                        }
                    }
                    else if (dbPassword != password) MessageBox.Show("비밀번호가 다릅니다.");
                }
                else MessageBox.Show("가입된 회원정보가 없습니다.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new JoinForm().ShowDialog();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = true;
            }
        }
    }
}
