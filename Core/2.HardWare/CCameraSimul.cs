using System;

namespace Core.HardWare
{
    //22.05.02 LYK CCameraGigE Class
    /// <summary> 
    /// 카메라가 PC에 연결되어 있지 않을때 사용되는 클래스
    /// 삭제 예정
    /// </summary>
    public sealed class CCameraSimul : CCamera
    {
        public int m_nCamNumber = 0;

        public CCameraSimul()
        {
            
        }

        public override void Dispose()
        {
            Uninitialize();     
        }

        public override bool Initialize(CCamera _Camera)
        {
            //m_sSerialNumber = sSerialNumber;    //220502 LYK 시리얼 번호 할당
            return m_bIsIntialized = true;
        }

        public override void Uninitialize()
        {
            if(m_bIsIntialized )
            {
                if (m_bIsConnected)
                    DisConnect();               //220502 LYK Camera DisConnect

                m_bIsIntialized = false;
            }
        }

        public override bool Connect(CCameraRecipe _Recipe)
        {
            return m_bIsConnected;
        }

        public override void DisConnect()
        {
            if(m_bIsConnected )
            {
                if (m_bIsGrab)
                    GrabHalt();                                    


                m_bIsConnected = false;
            }
        }

        public override void GrabStart()
        {
        }

        public override void GrabHalt()
        {

        }

        public override void CameraSetting(CCameraRecipe _Recipe)
        {
        }
        public override bool IsConnected()
        {
            return false;
        }
    }
}
