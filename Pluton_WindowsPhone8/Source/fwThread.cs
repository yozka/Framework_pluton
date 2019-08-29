using System;
using System.Windows.Threading;



namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------





    ///=====================================================================================
    ///
    /// <summary>
    /// вызов функции в потоке интерфейса
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public static class UIThread
    {
        private static Dispatcher mDispatcher = null;

        static UIThread()
        {
        }

        public static void init(Dispatcher dispatcher)
        {
            mDispatcher = dispatcher;
        }

        public static void invoke(Action action)
        {
            //action.Invoke();
            if (mDispatcher != null)
            {
                mDispatcher.BeginInvoke(action);
            }
        }
    }
    ///--------------------------------------------------------------------------------------






}