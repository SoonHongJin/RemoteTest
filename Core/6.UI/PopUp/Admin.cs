using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Core.Program;

namespace Core.UI
{
    public partial class Admin : Form
    {
        string selectedPath = "";

        public Admin()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.SelectedPath = DEF_SYSTEM.DEF_FOLDER_PATH_IMAGE;


            if (fbd.ShowDialog() == DialogResult.OK)
            {
                selectedPath = fbd.SelectedPath;
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(selectedPath);

                listBox1.Items.Clear();
                foreach (var item in di.GetDirectories())
                {
                    listBox1.Items.Add(item.Name);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string selectFolderName = listBox1.SelectedItem.ToString();
            string imagePath = selectedPath + "\\" + selectFolderName;
            //string[] number = selectFolderName.Split('_');

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(imagePath);
            System.IO.FileInfo[] files = di.GetFiles();

            string fileName = files[0].Name;

            string[] number = fileName.Split('_');

            int ListImageIndex = 0;

            //theMainSystem.SimulTest(imagePath, number[0]);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            theMainSystem.tempFileName = txt_fileName.Text;

            txt_fileName.Text = "";
        }
    }
}
