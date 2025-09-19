using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json.Linq;

using static Core.Program;
using Core.Utility;

namespace Core.UI
{
    public partial class FormLogin : Form
    {
        private List<string[]> UserInfo = new List<string[]>();
        private string sPath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\User.json";
        private FormSideScreen formSideScreen;
        private FormTopScreen formTopScreen;
        public FormLogin(FormSideScreen formSideScreen)
        {
            InitializeComponent();
            this.formSideScreen = formSideScreen;
            rb_Operator.Checked = true;

            LoadUserInfo();
        }

        

        private void btn_Login_Click(object sender, EventArgs e)
        {
            //JObject users = JObject.Parse(File.ReadAllText(sPath));
            //string selectedID = "";
            //string selectedPassword = "";

            //if (rb_Operator.Checked)
            //{
            //    selectedID = users["Operator"]["ID"].ToString();
            //    selectedPassword = users["Operator"]["Password"].ToString();
            //}
            //else if (rb_Master.Checked)
            //{
            //    selectedID = users["Master"]["ID"].ToString();
            //    selectedPassword = users["Master"]["Password"].ToString();
            //}
            //else
            //{
            //    MessageBox.Show("Please Check Access");
            //    return;
            //}

            //if (tb_ID.Text == selectedID && tb_PassWord.Text == selectedPassword)
            //{
            //    MessageBox.Show("Login Success");
            //    formSideScreen.SetUserInfo(tb_ID.Text, rb_Operator.Checked ? "Operator" : "Master");
            //    formSideScreen.SetAccess(rb_Operator.Checked ? "Operator" : "Master");
            //    this.Close();
            //}
            //else
            //{
            //    MessageBox.Show("Check ID and Password");
            //}

            string Id = tb_ID.Text;
            string Password = tb_PassWord.Text;
            string Access = rb_Operator.Checked ? "Operator" : "Master";
            bool LoginSuccess = false;
            int CompanyNum = 0;
            for (int i = 0; i < UserInfo.Count; i++)
            {
                if (Id == UserInfo[i][0] && Password == UserInfo[i][1] && Access == UserInfo[i][3])
                {
                    LoginSuccess = true;
                    CompanyNum = i;
                    break;
                }
            }

            if (LoginSuccess)
            {
                MessageBox.Show("Login Success");
                this.Close();
            }
            else
            {
                MessageBox.Show("Login Failed, Please check your Id and Password");
            }

            
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void LoadUserInfo()
        {
            string sPath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\UserInfo.csv";
            if (File.Exists(sPath))
            {
                string[] lines = File.ReadAllLines(sPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string[] userInfo = line.Split(',');
                    UserInfo.Add(userInfo);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                tb_PassWord.PasswordChar = default(char);
            }
            else
            {
                tb_PassWord.PasswordChar = '*';
            }
        }
    }
}
