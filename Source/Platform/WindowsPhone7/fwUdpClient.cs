#region Using framework
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
        private const int   cMAX_BUFFER = 1024 * 50;
        
        private Socket      mUdp        = null;
        private IPEndPoint  mAddress    = null;
        private int         mPort       = 0;

        private bool        mSending    = false;
        private bool        mReceiving  = false;
        private string      mError      = null;

        private byte[]                  mBufferSend     = null; //буфер для отправки данных
        private readonly byte[]         mBufferRecev    = new byte[cMAX_BUFFER];
        private readonly byte[]         mBufferTransfer = new byte[cMAX_BUFFER];

        private SocketAsyncEventArgs    mSocketSender   = null;
        private SocketAsyncEventArgs    mSocketRecev    = null;
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

            mPort = port;
            IPAddress address = null;
            if (!IPAddress.TryParse(ipString, out address))
            {
                return false;
            }

            try
            {
                mAddress = new IPEndPoint(address, port);
                mUdp = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                mSocketSender = new SocketAsyncEventArgs();
                mSocketSender.RemoteEndPoint = mAddress;
                mSocketSender.Completed += slot_sendCompleted;

                mUdp.ConnectAsync(mSocketSender);

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
        /// Отсоеденение от сервера
        /// </summary>
        ///--------------------------------------------------------------------------------------
        public void disconnect()
        {
            if (mUdp != null)
            {
                mUdp.Close();
                mUdp = null;
            }

            mAddress = null;
            mSending = false;
            mReceiving = false;

            mSocketRecev = null;
            mSocketSender = null;
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

            if (mUdp == null || mSending || buffer == null || mSocketSender == null)
            {
                return false;
            }
            try
            {
                mSending = true;

                if (mBufferSend == null || mBufferSend.Length < length)
                {
                    mBufferSend = new byte[length];
                }
                Array.Copy(buffer, mBufferSend, length);
                
                
                
                //mSocketSender.SetBuffer(mBufferSend, 0, length);

                var sender = new SocketAsyncEventArgs();
                sender.RemoteEndPoint = mAddress;
                sender.SetBuffer(mBufferSend, 0, length);
                
                sender.Completed += slot_sendCompleted;


                mUdp.SendToAsync(sender);
                //mUdp.SendAsync(mSocketSender);
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
        private void slot_sendCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (mSocketRecev == null)
            {
                try
                {
                    mSocketRecev = new SocketAsyncEventArgs();
                    mSocketRecev.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0); //mAddress;
                    mSocketRecev.Completed += slot_recevCompleted;
                    mSocketRecev.SetBuffer(mBufferRecev, 0, cMAX_BUFFER);
                    mUdp.ReceiveFromAsync(mSocketRecev);
                    //mUdp.SendToAsync(mSocketSender);
                }
                catch (Exception ex)
                {
                    mError = ex.Message;
                }
            }

            mSending = false;
        }
     
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// прием данных
        /// </summary>
        ///--------------------------------------------------------------------------------------
        private void slot_recevCompleted(object sender, SocketAsyncEventArgs e)
        {
            mReceiving = true;
            try
            {
                int start   = e.Offset;
                int length  = e.BytesTransferred;
                Array.Copy(e.Buffer, start, mBufferTransfer, 0, length);


                mSocketRecev = new SocketAsyncEventArgs();
                mSocketRecev.RemoteEndPoint = new IPEndPoint(IPAddress.Any, 0); //mAddress;
                mSocketRecev.Completed += slot_recevCompleted;
                mSocketRecev.SetBuffer(mBufferRecev, 0, cMAX_BUFFER);


                mUdp.ReceiveFromAsync(mSocketRecev);
                
                if (signal_receive != null && length > 0)
                {
                    signal_receive(mBufferTransfer, length);
                }

            }
            catch (Exception ex)
            {
                mError = ex.Message;
            }
            mReceiving = false;
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
