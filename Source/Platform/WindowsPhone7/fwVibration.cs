using System;
using Microsoft.Devices;
using Microsoft.Xna.Framework;



namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------







    ///=====================================================================================
    ///
    /// <summary>
    /// ������� ��������
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AVibrationDevice
    {
        ///--------------------------------------------------------------------------------------
        private bool m_vibration = false;
        private VibrateController vc = VibrateController.Default;
        private int[] m_current = null; //������� ������ ������� ��������
        private int m_index = 0;//������� ������ ������������� � �������
        private TimeSpan m_timeNext = TimeSpan.Zero;


        /* �������� ��������� �������� � ������������
         * ������ ����� - ��������
         * �������� ����� - ��������
         * ------------------------------------------------- #       #       #
         */
        private readonly int[] m_vibSelectMenu = new int[] { 50, 100, 100 };
        private readonly int[] m_vibGetCheese = new int[] { 100, 100, 100 };
        private readonly int[] m_vibNewCat = new int[] { 50, 50, 50 };
        private readonly int[] m_vibCatAttacs = new int[] { 50, 100, 100, 100, 50 };
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ��������� ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool enabled
        {
            get
            {
                return m_vibration;
            }
            set
            {
                m_vibration = value;
                if (!m_vibration)
                {
                    m_index = 0;
                    m_timeNext = TimeSpan.Zero;
                    m_current = null;
                }
            }
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// ��������, ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void update(TimeSpan gameTime)
        {
            if (m_vibration && m_current != null)
            {
                m_timeNext -= gameTime;
                if (m_timeNext.TotalMilliseconds < 0)
                {
                    //��������� ����
                    int iLength = m_current.Length;
                    if (m_index < iLength)
                    {
                        //������������ ��������
                        int time = m_current[m_index];
                        vc.Start(TimeSpan.FromMilliseconds(time));

                        //������������ ����� ����� ��������
                        m_index++;
                        if (m_index < iLength)
                        {
                            time += m_current[m_index];//����� ����� �����
                            m_index++;//��������� ����
                        }
                        m_timeNext = TimeSpan.FromMilliseconds(time);
                    }

                    //��������, ���� ���� ��������, �� ������� ���
                    if (m_index >= iLength)
                    {
                        m_index = 0;
                        m_timeNext = TimeSpan.Zero;
                        m_current = null;
                    }
                }


            }

        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// ����� ������ ����
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void vibSelectMenu()
        {
            if (m_vibration)
            {
                m_index = 0;
                m_timeNext = TimeSpan.Zero;
                m_current = m_vibSelectMenu;
            }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// ���� ����� ���
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void vibGetCheese()
        {
            if (m_vibration)
            {
                m_index = 0;
                m_timeNext = TimeSpan.Zero;
                m_current = m_vibGetCheese;
            }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// �������� ����� ���
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void vibNewCat()
        {
            if (m_vibration)
            {
                m_index = 0;
                m_timeNext = TimeSpan.Zero;
                m_current = m_vibNewCat;
            }
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// ����������� �����, ������ �����
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void vibCatAttacs()
        {
            if (m_vibration)
            {
                m_index = 0;
                m_timeNext = TimeSpan.Zero;
                m_current = m_vibCatAttacs;
            }
        }


    }
}
