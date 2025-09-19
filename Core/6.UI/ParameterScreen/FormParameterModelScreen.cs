using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

using Core;
using Core.HardWare;
using Core.Function;
using Core.Utility;

using Insnex.Vision2D.Core;
using Insnex.Vision2D.Common;
using Insnex.Vision2D.ToolBlock;
using Insnex.Vision2D.ImageProcessing;
using Insnex.Vision2D.Finder;
using Insnex.Vision2D.Intersection;
using Insnex.Vision2D.CalibFix;
using Insnex.Vision2D.Measurement;

using static Core.Program;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using ScottPlot.Renderable;
using Core.DataProcess;

namespace Core.UI
{
    public partial class FormParameterModelScreen : Form
    {
        public static string ImageSaveLogic = "=,<,<=,>,>="; //240804 KCH Image Save Condition Logic 
        //240804 KCH Image Save Condition Defect Class
        public static string DefectClass = "InspTotalTime,DeepLearningTime,GrabCycleTime,ContourInspTime,CrackInspTime,ColorInspTime,ColorGrade,ColorThickness,FingerInterType,FingerInterCount,FingerKnotType,FingerKnotCount," +
                                            "Paste Stained Count,Paste Stained Accumlate Size,Scratch Count,Scratch Accumulate Size,Ladder Interruption Count,Ladder Interruption Accumulate Length,AlignX,AlignY,AlignT,FinalJudge";
        private MainForm MainForm = null;
        public enum ARRAY_NUM { One = 0, Two, Three, Four, Five };
        //private CModelData ModelData;
        private string m_sModelListSelectedName = string.Empty;
        private string m_sTempModelName = string.Empty;
        private string m_sModelNumber = string.Empty;

        public string[] SetDefectClass; //240804 KCH Image Save Condition Logic 저장할 문자열 배열
        public string[] SetImageSaveLogic; //240804 KCH Image Save Condition Defect 저장할 문자열 배열

        //250122 NWT Parameter Exception
        private List<CExceptionData> exceptionDataList = new List<CExceptionData>();
        private ParameterExceptionHandler parameterExceptionHandler = null;
        private CLogger Logger = null;

        public FormParameterModelScreen(MainForm _mainForm, CLogger _logger)
        {
            InitializeComponent();
            TopLevel = false;
            MainForm = _mainForm;
            Logger = _logger;
            exceptionDataList = theRecipe.m_listExceptionData;
            MainForm.SetEachControlResize(this);    //240801 NIS Conrol Resize

            theMainSystem.RefreshModelList = RefreshModelList;
            RefreshModelList();

            DoLoad();

            Logger = _logger;
        }

        private void FormParameterModelScreen_Enter(object sender, EventArgs e)
        {
            RefreshModelList();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            theRecipe.DataSave(theRecipe.m_sCurrentModelName);

        }




        // Model List °ü·Ã
        #region ModelList

        // Model List¿¡¼­ ¼±ÅÃÇÑ ¸ðµ¨ÀÌ¸§
        private void lst_ModelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lst_ModelList.SelectedItem.ToString() != "")
            {
                m_sTempModelName = lst_ModelList.SelectedItem.ToString();
                string[] split = m_sTempModelName.Split('_');

                m_sModelNumber = split[0];

            }
        }

        // Model Load
        private void btn_LoadModel_Click(object sender, EventArgs e)
        {
            // DB
            /*
            CMainFrame.DataManager.ChangeModel(m_sModelListSelectedName);
            ModelData = ObjectExtensions.Copy(CMainFrame.DataManager.ModelData);

            CMainFrame.SideScreen.txt_Vision_Model.Text = ModelData.ModelName;
            txt_InspectionPoint.Text = Convert.ToString(ModelData.InspectionPoint);
            */

            //theRecipe.ModelCahnge(m_sModelNumber);
            theRecipe.ModelCahnge(m_sTempModelName);

            theMainSystem.RefreshCurrentModelName?.Invoke(theRecipe.m_sCurrentModelName);
            DoLoad();


        }
        #endregion


        // ¼±ÅÃÇÑ ¸ðµ¨ »èÁ¦
        private void btn_DeleteModel_Click(object sender, EventArgs e)
        {
            string sSelectedItem = Convert.ToString(lst_ModelList.SelectedItem);

            if (sSelectedItem == theRecipe.m_sCurrentModelName)
            {
                MessageBox.Show("matches the current model");
            }
            else
            {
                //241121 NWT Model Delete 체크 메세지 추가
                DialogResult ModelDeletechk = MessageBox.Show("Are you sure you want to delete this model?", "Delete Check", MessageBoxButtons.YesNo);
                if (ModelDeletechk == DialogResult.Yes)
                {
                    string sModelFilePath = DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\" + sSelectedItem;

                    System.IO.DirectoryInfo dirRecipe = new System.IO.DirectoryInfo(sModelFilePath);
                    dirRecipe.Delete(true);    // false·Î ÇÒ °æ¿ì ÇÏÀ§ Æú´õ¿Í Æú´õ ³»ÀÇ ÆÄÀÏµéÀº »èÁ¦°¡ ¾ÈµÈ´Ù.

                    RefreshModelList();
                }
            }

        }

        //240901 NWT AutoDelete Radio Button Load
        public void SetAutoDelete()
        {
            if (theRecipe.m_nAutoDeleteMode == 0)
            {
                rb_DateLimit.Select();
            }
            else
            {
                rb_DriveLimit.Select();
            }
        }
        // ModelList »õ·Î°íÄ§
        private void btn_RefreshModel_Click(object sender, EventArgs e)
        {
            RefreshModelList();
        }

        // Model List »õ·Î°íÄ§
        private void RefreshModelList()
        {
            // NOTE : Recipe List ÆäÀÌÁö¿¡ List ³»¿ëÀ» ÃÊ±âÈ­, Json ÆÄÀÏÀÌ ÀúÀåµÇ¾î ÀÖ´Â Æú´õ¿¡¼­ ÆÄÀÏ ¸ñ·ÏÀ» °¡Á®¿Í ¸®½ºÆ®¿¡ Ãâ·Â.

            lst_ModelList.Items.Clear();

            string[] fileEntries = Directory.GetDirectories(DEF_SYSTEM.DEF_FOLDER_PATH_RECIPE + "\\");
            fileEntries.OrderBy(ss => new FileInfo(ss).Name);

            foreach (string filePathName in fileEntries)
            {
                string fileName = Path.GetFileName(filePathName);
                lst_ModelList.Items.Add(fileName);
            }

        }

        private void btn_NewModel_Click(object sender, EventArgs e)
        {
            CreateModel CreateModelScreen = new CreateModel(this, 0, Logger);
            CreateModelScreen.m_nModelMode = 0;
            DialogResult dialogResult = CreateModelScreen.ShowDialog();

            if (DialogResult.OK == dialogResult)
                RefreshModelList();
        }

        private void btn_CopyModel_Click(object sender, EventArgs e)
        {
            CreateModel CreateMdoelScreen = new CreateModel(this, 1, Logger);
            DialogResult dialogResult = CreateMdoelScreen.ShowDialog();

            if (DialogResult.OK == dialogResult)
                RefreshModelList();
        }

        private void btn_RenameModel_Click(object sender, EventArgs e)
        {
            CreateModel CreateMdoelScreen = new CreateModel(this, 2, Logger);
            DialogResult dialogResult = CreateMdoelScreen.ShowDialog();

            if (DialogResult.OK == dialogResult)
                RefreshModelList();
        }

        private void DoLoad()
        {
            SetDefectClass = DefectClass.Split(','); //240804 KCH Defect Class 문자열 분리후 배열에 저장
            SetImageSaveLogic = ImageSaveLogic.Split(','); //240804 KCH Save Logic 문자열 분리후 배열에 저장
        }

        private void btn_CamSetApplySave_Click(object sender, EventArgs e)
        {
            SetData();

            //250404 LYK 임시 주석
            //theMainSystem.Cameras[DEF_SYSTEM.CAM_ONE].DeeplearningInsp.InspectionOptionSet(theRecipe.m_nDefectScoreThreshHold, theRecipe.m_nDefectAreaThreshHold);

            theRecipe.DataSave(theRecipe.m_sCurrentModelName);
        }

        private void SetData()
        {
            SaveConditionRecipe(); //240804 KCH 세팅한 이미지 저장 조건을 Recipe의 ConditionList에 저장하는 함수
        }
        public void SaveConditionRecipe() //240804 KCH 세팅한 이미지 저장 조건을 Recipe의 ConditionList에 저장하는 함수
        {
            try
            {
                theRecipe.ImageSaveConditionList.Clear();
                for (int rowIndex = 0; rowIndex < dg_ConditionSet.Rows.Count; rowIndex++)
                {
                    DataGridViewRow row = dg_ConditionSet.Rows[rowIndex];

                    string[] rowData = new string[row.Cells.Count];
                    for (int cellIndex = 0; cellIndex < row.Cells.Count; cellIndex++)
                    {
                        //240812 KCH 셀 값이 null이거나 "" 경우 False로 설정
                        if (row.Cells[cellIndex].Value == null || row.Cells[cellIndex].Value.ToString() == "")
                        {
                            rowData[cellIndex] = "False";
                        }
                        else
                        {
                            rowData[cellIndex] = row.Cells[cellIndex].Value.ToString();//240812 KCH 셀 값을 문자열로 변환하여 저장

                        }
                    }
                    theRecipe.ImageSaveConditionList.Add(rowData); //240812 KCH 변환된 행 데이터를 ConditionList에 추가
                }

            }
            catch
            {

            }
        }

        public void SetDataGrid() //240804 KCH DataGridView를 초기화하고 ConditionList의 데이터를 설정하는 함수
        {
            try
            {
                dg_ConditionSet.Rows.Clear();

                if (theRecipe.ImageSaveConditionList.Count <= 0) //240812 KCH Condition 리스트 Default추가 조건
                {
                    theRecipe.ImageSaveConditionList.Add(theRecipe.NgImageSaveCondition);
                    theRecipe.ImageSaveConditionList.Add(theRecipe.OkImageSaveCondition);
                }
                for (int i = 0; i < theRecipe.ImageSaveConditionList.Count; i++)
                {
                    SetConditionComboBoxItems(); //240812 KCH 콤보 박스 아이템을 설정
                    string[] rowData = new string[dg_ConditionSet.ColumnCount];
                    for (int j = 0; j < dg_ConditionSet.ColumnCount; j++)
                    {
                        rowData[j] = theRecipe.ImageSaveConditionList[i][j];
                    }
                    dg_ConditionSet.Rows.Add(rowData); //240812 KCH DataGridView에 새 행을 추가합니다.
                }

                for (int i = 0; i < 2; i++) //240812 KCH 첫 두 행에 대해 ReadOnly 설정을 적용
                {
                    DataGridViewRow row = dg_ConditionSet.Rows[i];
                    for (int j = 1; j < 5; j++) //240812 KCH 체크 박스 제외 항목을 모두 ReadOnly로 변경
                    {
                        row.Cells[j].ReadOnly = true;
                    }
                    row.Cells[10].ReadOnly = true;
                }
            }
            catch
            {

            }

        }
        public void SetConditionComboBoxItems() //240804 KCH Condition Setting의 [+]버튼 누를시 DataGridCell에 콤보 박스의 아이템에 DefectClass, SaveLogic 추가
        {
            dg_cb_ImageSaveCondition_Condition.Items.Clear();
            dg_cb_ImageSaveCondition_Condition.Items.AddRange(SetDefectClass);

            dg_cb_ImageSaveCondition_Logic.Items.Clear();
            dg_cb_ImageSaveCondition_Logic.Items.AddRange(SetImageSaveLogic);
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e) //240804 KCH Image Path를 더블 클릭 시 저장 경로 설점 Dialog Show
        {
            if (dg_ConditionSet.CurrentCell.ColumnIndex == 10 && dg_ConditionSet.CurrentCell.ReadOnly != true)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                fbd.SelectedPath = DEF_SYSTEM.DEF_FOLDER_PATH_IMAGE;
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    dg_ConditionSet.Rows[dg_ConditionSet.CurrentRow.Index].Cells[dg_ConditionSet.CurrentCell.ColumnIndex].Value = fbd.SelectedPath;
                }

            }
        }

        private void btn_AddConditionRow_Click(object sender, EventArgs e) //240804 KCH  Condition Seting의 [+]버튼 클릭
        {
            dg_ConditionSet.Rows.Add();
            SetConditionComboBoxItems();
        }

        private void btn_DeleteConditionRow_Click(object sender, EventArgs e) //240804 KCH  Condition Seting의 [-]버튼 클릭
        {
            if (dg_ConditionSet.Rows.Count > 2) //240812 KCH DataGridView에 최소 3개의 행이 있는지 확인
            {
                int lastRowIndex = dg_ConditionSet.Rows.Count - 1;
                if (lastRowIndex >= 2) //240812 KCH 마지막 행의 인덱스가 2 이상일 때만 삭제
                {
                    dg_ConditionSet.Rows.RemoveAt(lastRowIndex); //240812 KCH 마지막 Row 삭제
                }
            }
        }

        private void dg_ConditionSet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex >= 0)  //241004 NIS 첫번째 빈블록(ColumnIndex = -1) 선택 시 오류 발생
                {
                    DataGridViewCell cell = dg_ConditionSet[e.ColumnIndex, e.RowIndex];

                    if (cell.GetType() == typeof(DataGridViewComboBoxCell)) // 240812 KCH 셀의 타입을 확인하여 편집 모드로 전환
                    {
                        dg_ConditionSet.CurrentCell = cell; //240812 KCH 셀을 현재 셀로 설정
                        dg_ConditionSet.BeginEdit(true);    //240812 KCH 셀을 편집 모드로 전환
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        
        /// <summary>
        /// 25.01.22 NWT Parameter Click Event
        /// Parameter 클릭 시 keypad 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Click_Event(object sender, EventArgs e)
        {

        }

        private string previousText = string.Empty; //250123 NWT 이전 파라미터 저장용 변수
        /// <summary>
        /// 25.01.23 NWT Textbox의 Text가 변경되기 전 Data 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void text_MouseDown(object sender, MouseEventArgs e)    
        {
            if(e.Button == MouseButtons.Left)
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
                            if (parameterExceptionHandler.CheckData())
                            {
                                GetTextBox.Text = tempText;
                                break;
                            }
                            else
                            {
                                GetTextBox.Text = previousText;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}