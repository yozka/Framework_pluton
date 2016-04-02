#region Using framework
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.GamerServices;
#endregion


namespace Pluton.SystemProgram.Devices
{



    ///=====================================================================================
    ///
    /// <summary>
    /// ���������� ����� ���������������� ����������.
    /// ����� ���������� � ������ ��������
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AInputDevice : IDisposable
    {

        //������� �� ���������
        private const int       c_touchMaxCount = 3;    //���������� ������������� ������� �� �����
        private Vector2[]       m_touch = null;         //���������� ������� ������������� � ������
        private int             m_touchCount = 0;       //���������� ������������� ������� �� �����

        private Vector2         mLastTouch = Vector2.Zero;


        //���������� ������
        private GamePadState    m_buttons;              //������� ������� ������
        private GamePadState    m_buttonsLast;          //���������� ������� ������


        //�����
        /*
        private Vector2         m_gesturesTap = Vector2.Zero;       //��������� �������
        private Vector2         m_gesturesTapLast = Vector2.Zero;   //��������� ������� ���������� �������
        */


        /*
        //������������
        private Vector2         m_accel = Vector2.Zero;//������������ ������
        private Vector2         m_zeroZone = Vector2.Zero;//������� ������� ���� ��� ����������
        private Vector3         m_avector = Vector3.Zero;//��������� ������ � �������������
        private Accelerometer   m_accelerometer = null;
        private Vector2         m_angle = new Vector2(1, 1);//������������� �������� ��� ������������ �������

        //������ ������� �������� �� ������������
        private const int       c_filterCount = 6;//���������� ������ � �������
        private Vector2[]       m_filter = null;//����� ������������ ������
        private int             m_filterID = 0;
        */
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AInputDevice(Game deviceGame)
        {
            /* ������� �� ������
             */
            m_touch = new Vector2[c_touchMaxCount];
            m_touchCount = 0;

            /* ���������� ������
             */
            m_buttons = new GamePadState();
            m_buttonsLast = new GamePadState();


            /* ������������
             */
            /*
            m_filter = new Vector2[c_filterCount];
            m_accelerometer = new Accelerometer();
            m_accelerometer.CurrentValueChanged += accelerometerChanged;
            m_accelerometer.Start();

            // �������� ��������� �������� �� ������������� ������
            deviceGame.Window.OrientationChanged += new EventHandler<EventArgs>(window_OrientationChanged);
            orientationChanged(deviceGame.Window.CurrentOrientation);
            */


            /* ������� ������
             */
            //TouchPanel.EnabledGestures = GestureType.Tap;
        }
        ///--------------------------------------------------------------------------------------





        
         ///=====================================================================================
        ///
        /// <summary>
        /// ������������ ��������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void Dispose()
        {
            /*
            m_accelerometer.Stop();
            m_accelerometer.Dispose();
             * */
        }
        ///--------------------------------------------------------------------------------------
       




         ///=====================================================================================
        ///
        /// <summary>
        /// ������ ���������� ��������� ���������� � ��������.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void update()
        {
            //������
            m_buttonsLast = m_buttons;
            m_buttons = GamePad.GetState(PlayerIndex.One);



            //�����
            /*
            m_gesturesTapLast = m_gesturesTap;
            if (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();
                switch (gs.GestureType)
                {
                    case GestureType.Tap: m_gesturesTap = gs.Position; break;
                }
            }
            else
            {
                m_gesturesTap = Vector2.Zero;
            }
            */

  
            //���������� �������������
            //��� ��������
            m_touchCount = 0;
            foreach (var item in TouchPanel.GetState())
            {
                if (item.State == TouchLocationState.Pressed
                    || item.State == TouchLocationState.Moved)
                {
                    // Get item.Position
                    mLastTouch = ASpriteBatch.fromViewPort(item.Position);
                    m_touch[m_touchCount] = mLastTouch;
                    m_touchCount++;
                    if (m_touchCount >= c_touchMaxCount)
                    {
                        //����� �� ������� ���������� �������
                        break;
                    }
                }
            }



        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// �������� �������� �����
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 lastTouch
        {
            get
            {
                return mLastTouch;
            }
        }
        ///--------------------------------------------------------------------------------------




        ///=====================================================================================
        ///
        /// <summary>
        /// �������� ��������� ����� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public delegate void eventInputBox(string value);
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ���� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void showInputBox(string title, string description, string value, eventInputBox signal)
        {
            IAsyncResult kbResult = Guide.BeginShowKeyboardInput(PlayerIndex.One, title, description, value, null, null);
            if (kbResult != null)
            {
                string text = Guide.EndShowKeyboardInput(kbResult);
                if (text != null && value != text && signal != null)
                {
                    signal(text);
                }
            }
        }
        ///--------------------------------------------------------------------------------------





     






         ///=====================================================================================
        ///
        /// <summary>
        /// ������������ ������� ����� ������.
        /// </summary>
        /// 
        public bool isNewButtonPress(Buttons button)
        {
            return (    m_buttons.IsButtonDown(button) &&
                        m_buttonsLast.IsButtonUp(button));
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ��������, ����������� ����� ����������� ������ ������ �� ���� ��� ���.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isMenuCancel()
        {
            return isNewButtonPress(Buttons.B) ||
                   isNewButtonPress(Buttons.Back);
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ��������, ����������� ����� ����������� ������ ������� ����� ��� ���.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isCancel()
        {
            return isNewButtonPress(Buttons.B) ||
                   isNewButtonPress(Buttons.Back);
        }
        ///--------------------------------------------------------------------------------------







 
        ///=====================================================================================
        ///
        /// <summary>
        /// ��������, ������������ ����� ����� ��� ��� �� ����� ����.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isPauseGame()
        {
            return isNewButtonPress(Buttons.Back) ||
                   isNewButtonPress(Buttons.Start);
        }
        ///--------------------------------------------------------------------------------------





        


         ///=====================================================================================
        ///
        /// <summary>
        /// ���������, ��������� �� ������� ����� � ��������������, ���������� ������ ���������� 
        /// �����
        /// �������� ������� ���������� -1, ����� ������ ��������� ����� ���������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int containsRectangle(int x, int y, int width, int height)
        {
            int xw = x + width;
            int yh = y + height;
            for (int i = 0; i < m_touchCount; i++)
            {
                Vector2 pos = m_touch[i];
                int pointX = (int)pos.X;
                int pointY = (int)pos.Y;
                if (pointX >= x && pointX < xw &&
                    pointY >= y && pointY < yh)
                {
                    return i;
                }
            }
            return -1;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ���������, ��������� �� ������� ����� � ��������������, ���������� ������ ���������� 
        /// �����
        /// �������� ������� ���������� -1, ����� ������ ��������� ����� ���������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int containsRectangle(Rectangle rect)
        {
            return containsRectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// ���������� ��������� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void release(int index)
        {
            if (index >= 0 && index < m_touchCount)
            {
                m_touch[index].X = -1;
                m_touch[index].Y = -1;
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ���������� ��� �������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void release()
        {
            for (int i = 0; i < m_touchCount; i++)
            {
                m_touch[i].X = -1;
                m_touch[i].Y = -1;
            }
            m_touchCount = 0;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ���������� �������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Point touch(int index)
        {
            if (index >= 0 && index < m_touchCount)
            {
                return m_touch[index].toPoint();
            }
            return Point.Zero;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ������ ������ ������� ������ ��� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int touchIndex()
        {
            if (m_touchCount >= 0)
            {
                for (int i = 0; i < m_touchCount; i++)
                {
                    if (m_touch[i].X >= 0 &&
                        m_touch[i].Y >= 0)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        ///--------------------------------------------------------------------------------------











         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ������ ���������� ��� �����
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isKeyLeft()
        {
            return false;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ������ ���������� ��� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isKeyRight()
        {
            return false;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ������ ���������� ��� �����
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isKeyUp()
        {
            return false;
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ������ ���������� ��� ����
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isKeyDown()
        {
            return false;
        }
        ///--------------------------------------------------------------------------------------








         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� �������������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        private void accelerometerChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            AccelerometerReading data = e.SensorReading;
            m_avector = new Vector3((float)data.Acceleration.X, (float)data.Acceleration.Y, (float)data.Acceleration.Z);

            // ������������ ����� �������
            m_filter[m_filterID] = new Vector2(m_avector.Y, m_avector.X) * m_angle - m_zeroZone;
            m_filterID++;
            if (m_filterID >= c_filterCount)
            {
                m_filterID = 0;
            }

            calcFilter();
        }*/
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ������� �������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        public void calcFilter()
        {
            m_accel = m_filter[0];
            for (int i = 1; i < c_filterCount; i++)
            {
                m_accel += m_filter[i];
            }
            m_accel = m_accel / c_filterCount;

        }*/
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ������� �������������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        public Vector2 acceleration
        {
            get
            {
                return m_accel;
            }
        }*/
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ����� �������������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        public Vector2 zeroZone
        {
            get
            {
                return m_zeroZone;
            }
            set
            {
                m_zeroZone = value;
            }
        }*/
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ������ ������� ���������� �������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        public Vector2 calibrationZeroZone()
        {
            return new Vector2(m_avector.Y, m_avector.X) * m_angle;
        }*/
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ��������� ������� �� ���������� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        public void window_OrientationChanged(object sender, EventArgs e)
        {
            GameWindow window = sender as GameWindow;
            if (window != null)
            {
                orientationChanged(window.CurrentOrientation);
            }
        }*/
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// ������������� �������������� ������ � ������ ���������� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        private void orientationChanged(DisplayOrientation orintat)
        {
            m_angle = new Vector2(-1, -1);
            if (orintat == DisplayOrientation.LandscapeRight)
            {
                m_angle = new Vector2(1, 1);
            }
        }
         * */




    }//AInputDevice
}