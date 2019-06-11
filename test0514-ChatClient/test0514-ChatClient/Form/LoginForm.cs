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
                MessageBox.Show("로그인시도중.");
                try
                {
                    dbPassword = UserDto.Users.Find(x => x.id == id).password;
                    
                    if (dbPassword==password)//로그인성공인 경우
                    {
                        textBox1.Text = ""; textBox2.Text = "";
                        this.Visible = false;
                        dto.Login(id);
                        MessageBox.Show("로그인성공임."+ LoginInfo.login.id);
                        if (new IndexForm().ShowDialog() == DialogResult.Cancel) { this.Visible = true; LoginInfo.login = null; this.Refresh(); MessageBox.Show("창닫았네?"+LoginInfo.login.id); }
                    }
                    else if (dbPassword != password) MessageBox.Show("비밀번호가 다릅니다.");
                    else MessageBox.Show("가입된 회원정보가 없습니다.");
                }
                catch {
                    MessageBox.Show("로그인 실패. 서버 관리자에게 확인하세요.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new JoinForm().ShowDialog();
        }
    }
}
