using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


using Pluton.SystemProgram;


namespace Pluton.GraphicsElement
{
    ///--------------------------------------------------------------------------------------







     ///=====================================================================================
    ///
    /// <summary>
    /// ������������ �������
    /// ���������� ��� �������� ��������
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AAnimationShow
    {
        ///--------------------------------------------------------------------------------------
        private float m_speed = 0;   //�������� �����������
        private float m_diff = 0;  //������� ��������� �� 0 �� 1;
        private ETypeState m_state = ETypeState.hide;
        ///--------------------------------------------------------------------------------------




        ///--------------------------------------------------------------------------------------
        /// <summary>
        /// ������� ��������� ��������� ������
        /// </summary>
        protected enum ETypeState
        {
            /// <summary>
            /// ������
            /// </summary>
            hide,

            /// <summary>
            /// �������� � ��������� 
            /// </summary>
            hideAnimat,


            /// <summary>
            /// ������� ����������
            /// </summary>
            show,

            /// <summary>
            /// �������� ��� ������
            /// </summary>
            showAnimat

        };
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// speed - �������� ����������������� �������� � ������������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AAnimationShow(float speed)
        {
            m_speed = 1 / speed;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ����������  ������� �������� �������� �� 0..1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public static implicit operator float(AAnimationShow p)
        {
            return p.m_diff;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// �������� �� ��� ��� �������� ��������� 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isHide()
        {
            return m_state == ETypeState.hide ? true : false;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// �������� ��������� ������� ��������������� ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isShow()
        {
            return m_state == ETypeState.show ? true : false;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ��������, ���� �� �������� ������ ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isShowing()
        {
            return m_state == ETypeState.showAnimat ? true : false;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ��������, ���� �� ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isAnimation()
        {
            switch (m_state)
            {
                case ETypeState.hideAnimat: return true;
                case ETypeState.showAnimat: return true;
            }
            return false;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ������ ������ ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void show()
        {
            m_state = ETypeState.showAnimat;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// ������ ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void hide()
        {
            m_state = ETypeState.hideAnimat;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// �������� ��� ���� �������� ���������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isHiding()
        {
            return m_state == ETypeState.hideAnimat ? true : false;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ������ �������� ����������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setHide()
        {
            m_diff = 0;
            m_state = ETypeState.hide;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ������� �������� ����������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void setShow()
        {
            m_diff = 1;
            m_state = ETypeState.show;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void update(TimeSpan gameTime)
        {
            switch (m_state)
            {
                case ETypeState.showAnimat:
                    {
                        m_diff += m_speed * (float)gameTime.TotalMilliseconds;
                        if (m_diff > 1)
                        {
                            m_diff = 1;
                            m_state = ETypeState.show;
                        }
                        break;
                    }

                case ETypeState.hideAnimat:
                    {
                        m_diff -= m_speed * (float)gameTime.TotalMilliseconds;
                        if (m_diff < 0)
                        {
                            m_diff = 0;
                            m_state = ETypeState.hide;
                        }
                        break;
                    }
            }
        }
        ///--------------------------------------------------------------------------------------









    }//AAnimationShow
}