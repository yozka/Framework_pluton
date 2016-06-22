﻿#region Using framework
using System;
using System.Net;
using System.Net.Sockets;
#endregion





namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------





  




     ///=====================================================================================
    ///
    /// <summary>
    /// Клиент датаграммы
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class AUdpClient
            
        : 
            IInteface_UdpClient
    {
        ///--------------------------------------------------------------------------------------
        private UdpClient   mUdp        = null;
        private IPEndPoint  mAddress    = null;

        private bool        mSending    = false;
        private bool        mReceiving  = false;
        private string      mError      = null;
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public AUdpClient()
        {
            
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// есть или нет ошибки
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public bool isError()
        {
            return (mError == null || mError == string.Empty) ? false : true;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// последняя ошибка
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public string lastError()
        {
            return mError;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// удалить ошибку
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public void clearError()
        {
            mError = null;
        }
        ///--------------------------------------------------------------------------------------







        ///=====================================================================================
        ///
        /// <summary>
        /// Проверка на предмет занятости сервера
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public bool isBussy()
        {
            if (mUdp == null)
            {
                return false;
            }

            return mSending || mReceiving;
        }
        ///--------------------------------------------------------------------------------------






        ///=====================================================================================
        ///
        /// <summary>
        /// Подсоеденение к серверу
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public bool connect(string ipString, int port)
        {
            mSending = false;
            mReceiving = false;


            IPAddress address = null;
            if (!IPAddress.TryParse(ipString, out address))
            {
                return false;
            }

            try
            {
                mAddress = new IPEndPoint(address, port);
                mUdp = new UdpClient();
                mUdp.Connect(mAddress);
                mUdp.BeginReceive(slot_receive, null);
            }
            catch (Exception ex)
            {
                mError = ex.Message;
                return false;
            }
            return true;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// прием данных
        /// </summary>
        ///--------------------------------------------------------------------------------------
        private void slot_receive(IAsyncResult ar)
        {
            if (mUdp == null)
            {
                return;
            }

            try
            {
                mReceiving = true;
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = mUdp.EndReceive(ar, ref remoteEP);



                if (signal_receive != null && remoteEP.Address.Equals(mAddress.Address))
                {
                    signal_receive(buffer, buffer.Length);
                }

                mReceiving = false;
                mUdp.BeginReceive(slot_receive, null);
            }
            catch (Exception ex)
            {
                mError = ex.Message;
                mReceiving = false;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Отсоеденение от сервера
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public void disconnect()
        {
            mUdp.Close();
            mUdp = null;
            mAddress = null;
            mSending = false;
            mReceiving = false;
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// Отсылка данных
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public bool send(byte[] buffer, int length)
        {
            if (mUdp == null && !mSending)
            {
                return false;
            }
            try
            {
                mSending = true;
                mUdp.BeginSend(buffer, length, slot_send, null);
            }
            catch (Exception ex)
            {
                mError = ex.Message;
                mSending = false;
                return false;
            }
            return true;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// Отсылка данных
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void slot_send(IAsyncResult ar)
        {
            if (mUdp == null)
            {
                return;
            }
            try
            {
                int sending = mUdp.EndSend(ar);
                mSending = false;
            }
            catch (Exception ex)
            {
                mError = ex.Message;
                mSending = false;
            }
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// сигнал, чтения данных
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public event eventUdpReceive signal_receive;
        ///--------------------------------------------------------------------------------------






    }
    ///--------------------------------------------------------------------------------------
}
