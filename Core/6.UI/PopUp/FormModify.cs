using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System.Text;

namespace Core.UI
{
    public partial class FormModify : Form
    {
        int Num = 0;
        private List<string[]> UserInfo = new List<string[]>();

        public FormModify()
        {
            InitializeComponent();
            rb_Operator.Checked = true;
        }

        private void btn_UserInfoApply_Click(object sender, EventArgs e)
        {
            //string[] NewUser = new string[4];
            //NewUser[0] = tb_ID.Text;
            //NewUser[1] = tb_PassWord.Text;
            //NewUser[2] = tb_Company.Text;
            //NewUser[3] = rb_Operator.Checked ? "Operator" : "Master";
            //
            //UserInfo.Add(NewUser);
            //SaveUserInfo();

            string newUser = $"{tb_ID.Text},{tb_PassWord.Text},{tb_Company.Text},{(rb_Operator.Checked ? "Operator" : "Master")}";
            SaveUserInfo(newUser);
            
        }

        public void SaveUserInfo(string newUser)
        {
            //string sPath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\UserInfo.csv";
            //using (StreamWriter file = new StreamWriter(sPath, false, Encoding.UTF8))
            //{
            //    for (int i = 0; i < UserInfo.Count; i++)
            //    {
            //        file.WriteLine("{0},{1},{2},{3}", UserInfo[i][0], UserInfo[i][1], UserInfo[i][2], UserInfo[i][3]);
            //    }
            //}

            string sPath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\UserInfo.csv";
            
            bool isduplicateID = false;
            using (StreamReader reader = new StreamReader(sPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] userInfo = line.Split(',');
                    if (userInfo.Length > 0 && userInfo[0] == tb_ID.Text)
                    {
                        isduplicateID = true;
                        break;
                    }
                    else
                    {
                        isduplicateID = false;
                    }
                }
            }
            if (!isduplicateID)
            { 
                using (StreamWriter file = new StreamWriter(sPath, true, Encoding.UTF8)) // 'true' for append mode
                {
                    file.WriteLine(newUser);
                }
                MessageBox.Show("Apply Success");
                this.Close();

            }
            else
            {
                MessageBox.Show("Duplicate ID");
            }
            
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}
