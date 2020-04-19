﻿#region Using framework
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
//using Microsoft.Xna.Framework.GamerServices;

using System.Threading.Tasks;

using System.Runtime.InteropServices;

#endregion

using System.Diagnostics;
using Microsoft.VisualBasic;

namespace Pluton.SystemProgram.Devices
{



    ///=====================================================================================
    ///
    /// <summary>
    /// Контроллер ввода пользовательской информации.
    /// Опрос тачпанелей и кнопок телефона
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AInputDevice
    {
        ///--------------------------------------------------------------------------------------
        public readonly static string platform = "WindowsGL";
        ///--------------------------------------------------------------------------------------



        //хардварные кнопки
        private KeyboardState   mButtons;              //текущие нажатые кнопки
        private KeyboardState   mButtonsLast;          //предыдущие нажатые кнопки

        //колесика прокрутки
        private int         mMouseWheel         = 0;            //колесико прокрутки
        private Vector2     mMouseTouch         = Vector2.Zero; //позиция мышки

        //нажатие на тачпанель
        private const int   c_touchMaxCount     = 3;            //количесвто одновременных нажатий на экран
        private Vector2[]   mTouch              = null;         //координаты нажатых прикосновений к экрану
        private int         mTouchCount         = 0;            //Количесвто одновременных нажатий на экран
        private Vector2     mLastTouch          = Vector2.Zero;

        private bool[]      mMouseLeftButton    = null;         //нажатие на левую кнопку мыши
        private bool[]      mMouseRightButton   = null;         //нажатиме на правую кнопку мыши
        ///--------------------------------------------------------------------------------------




#if RENDER_DEBUG
        public static bool testRenderDebug = false;
#endif




         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public AInputDevice(Game deviceGame)
        {
            
            /* нажатие на кнопки
             */
            mTouch = new Vector2[c_touchMaxCount];
            mTouchCount = 0;

            mMouseLeftButton    = new bool[c_touchMaxCount];
            mMouseRightButton   = new bool[c_touchMaxCount];

            /*
          
            TouchPanel.EnableMouseGestures = true;
            TouchPanel.EnableMouseTouchPoint = true;
            */
        
        }
        ///--------------------------------------------------------------------------------------





    

         ///=====================================================================================
        ///
        /// <summary>
        /// Чтение последнего состояния клавиатуры и геймпада.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void update()
        {
            //кнопки
            mButtonsLast = mButtons;
            mButtons = Keyboard.GetState();

#if RENDER_DEBUG
            if (isNewButtonPress(Keys.D))
            {
                testRenderDebug = !testRenderDebug;
            }
#endif


            mTouchCount = 0;

            MouseState ms = Mouse.GetState();
            mMouseTouch = ASpriteBatch.fromViewPort(new Vector2(ms.X, ms.Y));

            if (ms.LeftButton == ButtonState.Pressed ||
                ms.RightButton == ButtonState.Pressed)
            {
                mLastTouch                        = mMouseTouch;
                mTouch              [mTouchCount] = mLastTouch;
                mMouseLeftButton    [mTouchCount] = (ms.LeftButton == ButtonState.Pressed);
                mMouseRightButton   [mTouchCount] = (ms.RightButton == ButtonState.Pressed);

                mTouchCount++;
            }
 
     
    
            foreach (var item in TouchPanel.GetState())
            {
                if (item.State == TouchLocationState.Pressed
                    || item.State == TouchLocationState.Moved)
                {
                    // Get item.Position
                    mLastTouch = ASpriteBatch.fromViewPort(item.Position);
                    mTouch[mTouchCount] = mLastTouch;
                    mTouchCount++;
                    if (mTouchCount >= c_touchMaxCount)
                    {
                        //вышли за пределы количество нажатий
                        break;
                    }
                }
            }


            //колесико мышки
            mMouseWheel = ms.ScrollWheelValue;
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// конечная процедура ввода текста
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public delegate void eventInputBox(string value);
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// ввод текста
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void showInputBox(string title, string description, string value, eventInputBox signal)
        {
            //onShowInputBox(title, description, value, signal);
            /*
            string x = Interaction.InputBox(description, title, value);
            if (signal != null)
            {
                signal(x);
            }*/
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// Асинхронная версия ввода имени
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        /*
        private async void onShowInputBox(string title, string description, string value, eventInputBox signal)
        {
            string name = await KeyboardInput.Show(title, description, value);

            if (name != null && value != name)
            {
                if (signal != null)
                {
                    signal(name);
                }
            }
        }
         * */
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// коорданаты последней нажатой кнопки
        /// </summary>
        /// 
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
        /// Отслеживание нажатие новой кнопки.
        /// </summary>
        /// 
        public bool isNewButtonPress(Keys button)
        {
            return (    mButtons.IsKeyDown(button) &&
                        mButtonsLast.IsKeyUp(button));
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Отслеживание нажатие новой кнопки.
        /// </summary>
        /// 
        public bool isButtonPress(Keys button)
        {
            return mButtons.IsKeyDown(button);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Проверка, польователь нажал хардвардную кнопку выхода из меню или нет.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isMenuCancel()
        {
            return isNewButtonPress(Keys.Escape);
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Проверка, польователь нажал хардвардную кнопку нажатие назад или нет.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isCancel()
        {
            return isNewButtonPress(Keys.Escape) ||
                   isNewButtonPress(Keys.Back);
        }
        ///--------------------------------------------------------------------------------------







 
        ///=====================================================================================
        ///
        /// <summary>
        /// Проверка, пользователь нажал паузу или нет во время игры.
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isPauseGame()
        {
            return isNewButtonPress(Keys.P);
        }
        ///--------------------------------------------------------------------------------------





        


         ///=====================================================================================
        ///
        /// <summary>
        /// Проверяем, находится ли нажатые точки в прямоугольнике, возвращаем первую попавшуюся 
        /// точку
        /// вслучаии неудачи возвращаем -1, иначе индекс найденной точки координат
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int containsRectangle(int x, int y, int width, int height)
        {
            int xw = x + width;
            int yh = y + height;
            for (int i = 0; i < mTouchCount; i++)
            {
                Vector2 pos = mTouch[i];
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
        /// Проверяем, находится ли нажатые точки в прямоугольнике, возвращаем первую попавшуюся 
        /// точку
        /// вслучаии неудачи возвращаем -1, иначе индекс найденной точки координат
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
        /// Уничтожить указанный индекс
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void release(int index)
        {
            if (index >= 0 && index < mTouchCount)
            {
                mTouch[index].X = -1;
                mTouch[index].Y = -1;
            }
        }
        ///--------------------------------------------------------------------------------------




        ///=====================================================================================
        ///
        /// <summary>
        /// Уничтожить все индексы
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void release()
        {
            for (int i = 0; i < mTouchCount; i++)
            {
                mTouch[i].X = -1;
                mTouch[i].Y = -1;
            }
            mTouchCount = 0;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим координаты тачпада
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Point touch(int index)
        {
            if (index >= 0 && index < mTouchCount)
            {
                return mTouch[index].toPoint();
            }
            return Point.Zero;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим нажатали кнопка мыши левая
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool touchMouseLeft(int index)
        {
            if (index >= 0 && index < mTouchCount)
            {
                return mMouseLeftButton[index];
            }
            return false;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим нажатали кнопка мыши правая
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool touchMouseRight(int index)
        {
            if (index >= 0 && index < mTouchCount)
            {
                return mMouseRightButton[index];
            }
            return false;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим индекс первых нажатых данных для тчпада
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int touchIndex()
        {
            if (mTouchCount >= 0)
            {
                for (int i = 0; i < mTouchCount; i++)
                {
                    if (mTouch[i].X >= 0 &&
                        mTouch[i].Y >= 0)
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
        /// обработка кнопок клавиатуры ход слева
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isKeyLeft()
        {
            return mButtons.IsKeyDown(Keys.A);
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// обработка кнопок клавиатуры ход справа
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isKeyRight()
        {
            return mButtons.IsKeyDown(Keys.D);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// обработка кнопок клавиатуры ход вверх
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isKeyUp()
        {
            return mButtons.IsKeyDown(Keys.W);
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// обработка кнопок клавиатуры ход вниз
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isKeyDown()
        {
            return mButtons.IsKeyDown(Keys.S);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// обработка кнопок клавиатуры прыжо
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isKeyJump()
        {
            return mButtons.IsKeyDown(Keys.Space);
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// обработка акселерометра
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
#if (ACCELEROMETER)
        private void accelerometerChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            AccelerometerReading data = e.SensorReading;
            m_avector = new Vector3((float)data.Acceleration.X, (float)data.Acceleration.Y, (float)data.Acceleration.Z);

            /* высчитывание новой позиции
             */
            m_filter[m_filterID] = new Vector2(m_avector.Y, m_avector.X) * m_angle - m_zeroZone;
            m_filterID++;
            if (m_filterID >= c_filterCount)
            {
                m_filterID = 0;
            }

            calcFilter();
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// подсчет фильтра
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void calcFilter()
        {
            m_accel = m_filter[0];
            for (int i = 1; i < c_filterCount; i++)
            {
                m_accel += m_filter[i];
            }
            m_accel = m_accel / c_filterCount;

        }
#endif
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Позиция акселерометра
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 acceleration
        {
            get
            {
                #if (ACCELEROMETER)
                return m_accel;
                #else
                return Vector2.Zero;
                #endif
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// начальная точка позицирования
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 zeroZone
        {
            get
            {
                #if (ACCELEROMETER)
                return m_zeroZone;
                #else
                return Vector2.Zero;
                #endif
            }
            set
            {
                #if (ACCELEROMETER)
                m_zeroZone = value;
                #endif
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// выдача текущей калибровки датчика
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 calibrationZeroZone()
        {
            #if (ACCELEROMETER)
            return new Vector2(m_avector.Y, m_avector.X) * m_angle;
            #else
            return Vector2.Zero;
            #endif
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// Обработка события на ориентации экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void window_OrientationChanged(object sender, EventArgs e)
        {
            GameWindow window = sender as GameWindow;
            if (window != null)
            {
                orientationChanged(window.CurrentOrientation);
            }
        }
        ///--------------------------------------------------------------------------------------







         ///=====================================================================================
        ///
        /// <summary>
        /// устанавливаем корректирующие данные с учетом ориентации экрана
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void orientationChanged(DisplayOrientation orintat)
        {
            #if (ACCELEROMETER)
            m_angle = new Vector2(-1, -1);
            if (orintat == DisplayOrientation.LandscapeRight)
            {
                m_angle = new Vector2(1, 1);
            }
            #endif
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// запуск браузера
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool runBrowser(string url)
        {
            var prc = Process.Start(url);
            return true;
        }
        ///--------------------------------------------------------------------------------------





        ///=====================================================================================
        ///
        /// <summary>
        /// проверка, игра может выйти или нет
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public bool isExitGame()
        {
            return true;
        }
        ///--------------------------------------------------------------------------------------




        ///=====================================================================================
        ///
        /// <summary>
        /// Выход из игры
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void exitGame(Game game)
        {
            game.Exit();
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим колесико прокртуки
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public int wheelValue()
        {
            return mMouseWheel;
        }
        ///--------------------------------------------------------------------------------------




         ///=====================================================================================
        ///
        /// <summary>
        /// возвратим мышку
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public Vector2 touchMouse()
        {
            return mMouseTouch;
        }
        ///--------------------------------------------------------------------------------------





    }//AInputDevice
}