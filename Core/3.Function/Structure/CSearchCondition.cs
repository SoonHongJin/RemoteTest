using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Function
{
    public class CSearchCondition
    {
        //250204 KCH 카메라 조건 리스트
        public List<CameraCondition> CameraConditions { get; set; }

        //250204 KCH 생성자: CameraConditions 리스트 초기화
        public CSearchCondition()
        {
            CameraConditions = new List<CameraCondition>();
        }

        //250204 KCH 카메라 조건 추가
        public void AddCameraCondition(CameraCondition cameraCondition)
        {
            CameraConditions.Add(cameraCondition);
        }
    }

    //250204 KCH 카메라별 조건을 정의
    public class CameraCondition
    {
        public int CameraId { get; set; }  //250204 KCH 카메라 ID
        public bool IsEnabled { get; set; } //250204 KCH 카메라가 활성화 되어있는지 여부
        public List<Condition> Conditions { get; set; } //250204 KCH 카메라 조건 리스트

        //250204 KCH 생성자: 카메라 ID 설정 및 Conditions 리스트 초기화
        public CameraCondition(int cameraId)
        {
            CameraId = cameraId;
            IsEnabled = true;
            Conditions = new List<Condition>();
        }

        public void AddCondition(Condition condition)  //250204 KCH 조건 추가
        {
            Conditions.Add(condition);
        }

        public void ClearCondition() //250204 KCH 조건 초기화 (
        {
            Conditions.Clear();
        }

        //250204 KCH 조건을 정의하는 내부 클래스
        public class Condition
        {
            public string SelectColumn { get; set; }             //250204 KCH 검색할 컬럼명
            public bool ColumnEnable { get; set; }               //250204 KCH 컬럼 사용 여부
            public string SearchData { get; set; }               //250204 KCH 검색할 데이터
            public List<string> SelectedClassNames { get; set; } //250204 KCH 선택된 클래스 이름 리스트

            public double? Min { get; set; } //250204 KCH 최소값
            public double? Max { get; set; } //250204 KCH 최대값
        }
    }
}
