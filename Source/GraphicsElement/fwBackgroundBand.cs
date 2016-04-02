using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


using Pluton.SystemProgram;


namespace Pluton.GraphicsElement
{



     ///=====================================================================================
    ///
    /// <summary>
    /// ����������� ��������, ������ ������� ����� � ������, � ��������� �����
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ABackgroundBand
    {
        ///--------------------------------------------------------------------------------------
        private readonly float  c_speed     = 1f / 500f;   //�������� �����������
        private readonly float  c_height    = 0.10f;       //������ ������� � % �� ������
        private readonly float  c_alpha     = 0.50f; //0.75f

        private int             m_height    = 0;   //������ �������
        private float           m_diff      = 0f;  //������� ��������� �� 0 �� 1;
        private ETypeState      m_state     = ETypeState.hide;
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






         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor 1
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ABackgroundBand()
        {

        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// �������� �� ��������� ������ ������� ��� ���
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
        /// ������ ������ �������� �������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void showBand()
        {
            m_state = ETypeState.showAnimat;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// ������ �������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void hideBand()
        {
            m_state = ETypeState.hideAnimat;
        }
        ///--------------------------------------------------------------------------------------









         ///=====================================================================================
        ///
        /// <summary>
        /// �������� 
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void loadContent(ContentManager content)
        {
            
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// ��������� �������� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void render(ASpriteBatch spriteBatch)
        {
            switch (m_state)
            {
                case ETypeState.showAnimat:
                {
                    m_diff += c_speed * (float)spriteBatch.gameTime.TotalMilliseconds;
                    if (m_diff > 1)
                    {
                        m_diff = 1;
                        m_state = ETypeState.show;
                    }
                    break;
                }

                case ETypeState.hideAnimat:
                {
                    m_diff -= c_speed * (float)spriteBatch.gameTime.TotalMilliseconds;
                    if (m_diff < 0)
                    {
                        m_diff = 0;
                        m_state = ETypeState.hide;
                    }
                    break;
                }
            }


            //������������ ������� �� ������
            Point sizeScreen = ASpriteBatch.viewPort;
            if (m_height == 0)
            {
                m_height = (int)(sizeScreen.X * c_height);
            }

            float fAlpha = (c_alpha * m_diff);
            Color color = Color.Black;

            int iHeight =(int) (m_height * m_diff);

            spriteBatch.primitives.drawRectangle(new Rectangle(0, 0, sizeScreen.X, iHeight), color);
            spriteBatch.primitives.drawRectangle(new Rectangle(0, sizeScreen.Y - iHeight, sizeScreen.Y, iHeight), color);
            //spriteBatch.primitives.drawRectangle(new Rectangle(0, 0, sizeScreen.X, sizeScreen.Y), color * fAlpha);

        }
        ///--------------------------------------------------------------------------------------





        
         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ����� ������� ��� ������� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool updateTo(TimeSpan gameTime)
        {

            return false;
        }



    }//ABackgroundBand
}