using System;
using System.Collections;
using System.Windows.Forms;

namespace test0514_ChatServerApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string id = textBox1.Text;
            string password = textBox2.Text;
            if (id == "admin" || password =="1234")
            {
                textBox1.Text = ""; textBox2.Text = "";
                this.Visible = false;
                //if (new Form2().ShowDialog() == DialogResult.Cancel) this.Visible = true;
            }
            else MessageBox.Show("다시 입력해주세요.");
        }
    } //end Form1 class
} //end namespace