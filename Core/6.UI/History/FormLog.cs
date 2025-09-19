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

using Cognex.VisionPro;

using Core;

namespace Core.UI
{
    public partial class FormLog : Form
    {
        private string m_sCurrentDateInfo = null;
        private string m_sCurrentLogInfo = null;

        public FormLog()
        {
            InitializeComponent();
            TopLevel = false;

            LoadDateList();
        }

        private void LoadDateList()
        {
            string[] files = { "", };
            string[] SubFiles = { "", };
            string tempPath = null;
            DateListView.Clear();

            DirectoryInfo DirInfo = new DirectoryInfo(DEF_SYSTEM.DEF_FOLDER_PATH_LOG);

            if(DirInfo.Exists)
            {
                files = Directory.GetDirectories(DEF_SYSTEM.DEF_FOLDER_PATH_LOG);
                
                for (int i = 0; i < files.Length; ++i)
                {
                    tempPath = files[i];
                    SubFiles = Directory.GetDirectories(tempPath);

                    for(int j = 0; j < SubFiles.Length; ++j)
                    {
                        string[] DateSplit = SubFiles[j].Split('\\');
                        cbo_SelectDate.Items.Add(DateSplit[4] + "\\" + DateSplit[5]);
                    }                   
                }
            }
        }


        private void cbo_SelectDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] files = { "", };
            int nIdx = cbo_SelectDate.SelectedIndex;
            m_sCurrentDateInfo = cbo_SelectDate.Items[nIdx].ToString();

            DirectoryInfo DirInfo = new DirectoryInfo(DEF_SYSTEM.DEF_FOLDER_PATH_LOG);

            if (DirInfo.Exists)
            {
                files = Directory.GetFiles($"{DEF_SYSTEM.DEF_FOLDER_PATH_LOG}\\{m_sCurrentDateInfo}", "*.*", SearchOption.AllDirectories);

                for (int i = 0; i < files.Length; ++i)
                {
                    string DataSplit = files[i].Substring(files[i].LastIndexOf('\\') + 1);
                    DateListView.Items.Add(DataSplit);
                }
            }
        }

        private void DateListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] files = { "", };
            int nIdx = DateListView.FocusedItem.Index;
            m_sCurrentLogInfo = DateListView.Items[nIdx].SubItems[0].Text;
            StreamReader reader = null;

            Task.Run(() =>
            {
                using (FileStream file = new FileStream($"{DEF_SYSTEM.DEF_FOLDER_PATH_LOG}\\{m_sCurrentDateInfo}\\{m_sCurrentLogInfo}", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    reader = new StreamReader(file, System.Text.Encoding.GetEncoding(949));
                    string line = null;
                  
                    this.Invoke(new Action(delegate ()
                    {
                        LogDataListView.Clear();

                        while ((line = reader.ReadLine()) != null)
                        {                    
                            LogDataListView.Items.Add(line);
                        }

                        LogDataListView.Update();
                    })); 
                }
            });

        }

    }
}
