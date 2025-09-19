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

using static Core.Program;
using Core;
using Core.Utility;
using Core.DataProcess;

namespace Core.UI
{
    public partial class CreateModel : Form
    {
        public int m_nModelMode = 0;
        private FormParameterModelScreen ModelScreen = null;
        //250122 NWT Parameter Exception
        private List<CExceptionData> exceptionDataList = new List<CExceptionData>();
        private ParameterExceptionHandler parameterExceptionHandler = null;
        private CLogger Logger = null;
        public CreateModel(FormParameterModelScreen _ModelScreen, int _nModelMode, CLogger _logger)
        {
            InitializeComponent();
            ModelScreen = _ModelScreen;
            m_nModelMode = _nModelMode;
            exceptionDataList = theRecipe.m_listExceptionData;
            if (m_nModelMode == 0)
                RD_NewRecipe.Checked = true;
            else if (m_nModelMode == 1)
                RD_SaveAs.Checked = true;
            else
                RD_RenameAs.Checked = true;
            Logger = _logger;
        }

        private void BTN_ModelSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            CreateRecipe();
        }

        private void BTN_ModelCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RD_NewRecipe_CheckedChanged(object sender, EventArgs e)
        {
            m_nModelMode = 0;

            UIRefresh(m_nModelMode);

            TXT_ModelID.Text = "";
            TXT_ModelNum.Text = "";
        }

        private void UIRefresh(int _nModelMode)
        {
            if (m_nModelMode == 0)
            {
                CBO_ModelList.Visible = false;
                LBL_ModelList.Visible = false;

                RD_NewRecipe.Enabled = true;
                RD_SaveAs.Enabled = false;
                RD_RenameAs.Enabled = false;
            }
            else if(m_nModelMode == 1)
            {
                CBO_ModelList.Visible = true;
                LBL_ModelList.Visible = true;

                RD_SaveAs.Enabled = true;
                RD_NewRecipe.Enabled = false;
                RD_RenameAs.Enabled = false;
            } 
            else if(m_nModelMode == 2)
            {
                CBO_ModelList.Visible = true;
                LBL_ModelList.Visible = true;

                RD_RenameAs.Enabled = true;
                RD_NewRecipe.Enabled = false;
                RD_SaveAs.Enabled = false;
            }
        }

        private void RD_SaveAs_CheckedChanged(object sender, EventArgs e)
        {
            m_nModelMode = 1;

            UIRefresh(m_nModelMode);

            CBO_ModelList.Items.Clear();

            string[] fileEntries = Directory.GetDirectories(DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\");
            fileEntries.OrderBy(ss => new FileInfo(ss).Name);

            foreach (string filePathName in fileEntries)
            {
                string fileName = Path.GetFileName(filePathName);
                CBO_ModelList.Items.Add(fileName);
            }
            CBO_ModelList.SelectedIndex = 0;
        }



        private void RD_RenameAs_CheckedChanged(object sender, EventArgs e)
        {
            m_nModelMode = 2;

            UIRefresh(m_nModelMode);

            CBO_ModelList.Items.Clear();

            string[] fileEntries = Directory.GetDirectories(DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\");
            fileEntries.OrderBy(ss => new FileInfo(ss).Name);

            foreach (string filePathName in fileEntries)
            {
                string fileName = Path.GetFileName(filePathName);
                CBO_ModelList.Items.Add(fileName);
            }
            CBO_ModelList.SelectedIndex = 0;
        }

        private void CreateRecipe()
        {
            string message = string.Empty;
            int nNewModelNum = 0;
            
            string sSelectedValue = string.Empty;
            string[] sNmaeToken = { "", };

            //int nSelectedRecipeNum = Convert.ToInt32(sNmaeToken[0]);
            //string strSelectedRecipeNum = nSelectedRecipeNum.ToString("D2");
            int nSelectedRecipeNum = 0;
            string strSelectedRecipeNum = string.Empty;
            string strSelectedRecipeName = string.Empty;

            if (Int32.TryParse(TXT_ModelNum.Text, out nNewModelNum))
            {
                
            }
            else
            {
                MessageBox.Show("숫자를 입력 하세요");

                return;
            }
            
                
            string sNewModelNum = nNewModelNum.ToString("D2");
            string sNewModelID = TXT_ModelID.Text;
            

            if(m_nModelMode == 0)       //220513 LYK New Recipe
            {
                bool bCheckRecipeFolder = false;
                string sRecipeFolder = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\";

                DirectoryInfo DirInfo = new DirectoryInfo(sRecipeFolder);
                if(DirInfo.Exists)
                {
                    DirectoryInfo[] FolderNames = DirInfo.GetDirectories();

                    foreach(DirectoryInfo dInfo in FolderNames)
                    {
                        string[] DirectoryName = dInfo.Name.Split('_');
                        bool chknum = Int32.TryParse(DirectoryName[0], out int nRecipeNum);
                        //int nRecipeNum = Convert.ToInt32(DirectoryName[0]);
                        if (chknum)
                        {
                            if (nRecipeNum == nNewModelNum)
                            {
                                bCheckRecipeFolder = true;

                                break;
                            }
                        }
                    }
                }

                if(bCheckRecipeFolder == true)
                {
                    message = "The Recipe Number is Already Existed \n Please Input Another Number";

                    MessageBox.Show(message);

                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], message);


                    return;
                }
                else
                {
                    string sNewModelName = sNewModelNum + '_' + sNewModelID;
                    theRecipe.SaveRecipe(sNewModelName);
                }

            }
            else if(m_nModelMode == 1)
            {
                sSelectedValue = CBO_ModelList.SelectedItem.ToString();
                sNmaeToken = sSelectedValue.Split('_');
                nSelectedRecipeNum = Convert.ToInt32(sNmaeToken[0]);
                strSelectedRecipeName = sNmaeToken[1];
                strSelectedRecipeNum = nSelectedRecipeNum.ToString("D2");

                if (nSelectedRecipeNum != nNewModelNum) // 선택한 레시피 번호와 Save As 번호가 다를 때에만 저장
                {
                    // 새로 생성될 Recipe Folder 이름
                    string NewRecipeFolderName = sNewModelNum + "_" + sNewModelID;

                    //Recipe Data 복사
                    string NewRecipePath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\" + NewRecipeFolderName;   //Save As 로 새로이 복사 생성될 파일 명
                    string OldRecipePath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\" + sSelectedValue; // 복사할 대상 (목록에서 선택)

                    if (Directory.Exists(NewRecipePath) == false)
                        Directory.CreateDirectory(NewRecipePath);

                    // 원본 폴더에 있는 파일 List
                    string[] OldRecipeFilePaths = System.IO.Directory.GetFiles(OldRecipePath);

                    // 원본 폴더 내부의 파일을 전부 Copy
                    foreach (string OldRecipeFilePath in OldRecipeFilePaths)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string FileName = System.IO.Path.GetFileName(OldRecipeFilePath);
                        string NewFilePath = System.IO.Path.Combine(NewRecipePath, FileName);
                        File.Copy(OldRecipeFilePath, NewFilePath, true);
                        if (NewFilePath.Contains(".json"))  //250207 NWT json파일과, 딥러닝 모델 이름 변환
                            Directory.Move(NewFilePath, $"{NewRecipePath}\\{NewRecipeFolderName}.json");
                        else if (NewFilePath.Contains(".srCls"))
                            Directory.Move(NewFilePath, $"{NewRecipePath}\\{NewRecipeFolderName}.srCls");
                        else if (NewFilePath.Contains(".srSeg"))
                            Directory.Move(NewFilePath, $"{NewRecipePath}\\{NewRecipeFolderName}.srSeg");

                    }


                    Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "[RecipeList] Save As 버튼 클릭 \r New Recipe Name : " + NewRecipeFolderName);
                }
                else
                {
                    MessageBox.Show("Recipe Num " + sNewModelNum + " is Already Exists");
                }
            }
            else if(m_nModelMode == 2)
            {
                sSelectedValue = CBO_ModelList.SelectedItem.ToString();

                // 새로 생성될 Recipe Folder 이름
                string NewRecipeFolderName = TXT_ModelNum.Text + "_" + sNewModelID; // 변경할 레시피 이름

                // 현재 선택한 값이 현재 로드된 레시피 파일일 경우 현재 레시피의 이름까지 변경
                if ( theRecipe.m_sCurrentModelName == sSelectedValue )
                    theRecipe.m_sCurrentModelName = NewRecipeFolderName;

                // 레시피 이름 변경
                string NewRecipePath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\" + NewRecipeFolderName;   //Save As 로 새로이 복사 생성될 파일 명
                string OldRecipePath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\" + sSelectedValue;         // 변경 할 대상 (목록에서 선택)

                // 폴더 이름만 변경하므로 foreach 필요 없음
                Directory.Move(OldRecipePath, NewRecipePath);
                string FileName = System.IO.Path.GetFileName(OldRecipePath);
                string NewFilePath = System.IO.Path.Combine(NewRecipePath, FileName);
                Directory.Move(NewFilePath + ".json", $"{NewRecipePath}\\{NewRecipeFolderName}.json");  //250207 NWT json 파일과, 딥러닝 모델 이름 변환
                Directory.Move(NewFilePath + ".srCls", $"{NewRecipePath}\\{NewRecipeFolderName}.srCls");
                Directory.Move(NewFilePath + ".srSeg", $"{NewRecipePath}\\{NewRecipeFolderName}.srSeg");

                Logger.Logging(DEF_SYSTEM.LOGTYPE[(int)DEF_SYSTEM.LOGTYPE_ENUM.SystemLog], "[RecipeList] Rename As 버튼 클릭 \r Changed Recipe Name : " + NewRecipeFolderName);
            }

            theMainSystem.RefreshModelList?.Invoke();
            theMainSystem.RefreshModelComboList?.Invoke();
        }

        private void CBO_ModelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int SelectIndex = CBO_ModelList.SelectedIndex;

            if (SelectIndex != -1)
            {
                string SelectedRecipeaName = (string)CBO_ModelList.SelectedItem.ToString();
                string[] NameToken = SelectedRecipeaName.Split('_');

                string RecipeNum = NameToken[0];
                string RecipeName = NameToken[1];

                TXT_ModelNum.Text = RecipeNum;
                TXT_ModelID.Text = RecipeName;

            }
            else
            {
                MessageBox.Show("Please Select Recipe List Item.");
            }
        }

        private void TXT_ModelNum_DoubleClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 25.01.22 NWT Parameter Click Event
        /// Parameter 클릭 시 keypad 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Click_Event(object sender, EventArgs e)
        {
            TextBox tempTxtBox = (TextBox)sender;
            string strCurrent = "", strModify = "";
            strCurrent = tempTxtBox.Text;

            if (!MainForm.GetKeyPad(strCurrent, out strModify))
                return;

            for (int i = 0; i < exceptionDataList.Count; i++)
            {
                if (exceptionDataList[i].Tag.Contains(tempTxtBox.Name))
                {
                    using (parameterExceptionHandler = new ParameterExceptionHandler(exceptionDataList[i].Page, exceptionDataList[i].Name, exceptionDataList[i].Tag, exceptionDataList[i].Min, exceptionDataList[i].Max, exceptionDataList[i].DataType, strModify))
                    {
                        tempTxtBox.Text = parameterExceptionHandler.CheckData() ? strModify : strCurrent;
                        break;
                    }
                }
            }
        }

        private string previousText = string.Empty; //250123 NWT 이전 파라미터 저장용 변수
        /// <summary>
        /// 25.01.23 NWT Textbox의 Text가 변경되기 전 Data 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void text_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TextBox textBox = (TextBox)sender;
                previousText = textBox.Text;
            }
        }
        /// <summary>
        /// 25.01.23 NWT Parameter 값을 확인하여 기준값보다 크거나 작을경우 경고창 발생
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textChanged(object sender, EventArgs e)    //250123 NWT 파라미터 값 확인
        {
            TextBox GetTextBox = (TextBox)sender;
            string tempText = GetTextBox.Text;
            for (int i = 0; i < exceptionDataList.Count; i++)
            {
                if (exceptionDataList[i].Tag.Contains(GetTextBox.Name))
                {
                    if (string.IsNullOrEmpty(tempText) == false)
                    {
                        using (parameterExceptionHandler = new ParameterExceptionHandler(exceptionDataList[i].Page, exceptionDataList[i].Name, exceptionDataList[i].Tag, exceptionDataList[i].Min, exceptionDataList[i].Max, exceptionDataList[i].DataType, tempText))
                        {
                            GetTextBox.Text = parameterExceptionHandler.CheckData() ? tempText : previousText;
                            break;
                        }
                    }
                }
            }
        }

        private void Click_Event_Keyboard(object sender, EventArgs e)
        {
            TextBox tempTxtBox = (TextBox)sender;
            string strCurrent = "", strModify = "";
            strCurrent = tempTxtBox.Text;

            if (!MainForm.GetKeyboard(out strModify, "Input Model Name", false))
                return;

            for (int i = 0; i < exceptionDataList.Count; i++)
            {
                if (exceptionDataList[i].Tag.Contains(tempTxtBox.Name))
                {
                    using (parameterExceptionHandler = new ParameterExceptionHandler(exceptionDataList[i].Page, exceptionDataList[i].Name, exceptionDataList[i].Tag, exceptionDataList[i].Min, exceptionDataList[i].Max, exceptionDataList[i].DataType, strModify))
                    {
                        tempTxtBox.Text = parameterExceptionHandler.CheckData() ? strModify : strCurrent;
                        break;
                    }
                }
            }
        }
    }
}
