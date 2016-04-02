#region Using framework
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
#endregion





namespace Pluton.SystemProgram.Devices
{
    ///--------------------------------------------------------------------------------------
    ///--------------------------------------------------------------------------------------





     ///=====================================================================================
    ///
    /// <summary>
    /// ������� ������ � �������� ��������
    /// </summary>
    /// 
    ///--------------------------------------------------------------------------------------
    public class ANetworkWeb
    {
        ///--------------------------------------------------------------------------------------
        private static readonly int     cTimeOut    = 10000; //����� ������� ���� ��� ��������� ������� ����������� �������
        ///--------------------------------------------------------------------------------------





        ///--------------------------------------------------------------------------------------
        private bool        mLoginAth   = false;    //������� ����������� �� �������
        private int         mDeviceID   = 0;        //����������� ������� � ����
        private string      mDeviceGuid;            //���������� ����� �������
        

        private WebClient       mWebClient  = null;     //����������� ����� �������
        private List<AWebQuery> mPool = new List<AWebQuery>(); //���� ����������� ������

        private TimeSpan    mTimeWait   = TimeSpan.Zero;  //����� ������� ����, ����� ������
        private bool        mWait       = false;              //���� ���� ��� ����� ����� 
        ///--------------------------------------------------------------------------------------





     


         ///=====================================================================================
        ///
        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public ANetworkWeb(string deviceGuid)
        {
            mDeviceGuid = deviceGuid;
            mWebClient = new WebClient();
            mWebClient.UploadStringCompleted += evCommandCompleted;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// �������� ������ �������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void send(AWebQuery command)
        {
            command.sendQueue();
            mPool.Add(command);
            executeCommand();
        }
        ///--------------------------------------------------------------------------------------
  






         ///=====================================================================================
        ///
        /// <summary>
        /// ���� �� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void deviceAuthorization()
        {
            if (mLoginAth)
            {
                return;
            }
            mLoginAth = true;
            AWebCommand cmd = new AWebCommand("ath");
            cmd.parameters.addString("deviceGuid", mDeviceGuid);
            string data = cmd.toString();

            mWebClient.Headers["Content-Type"] = "application/x-www-form-urlencoded";
            mWebClient.Encoding = Encoding.UTF8;
            mWebClient.UploadStringAsync(AWebCommand.cURI, "POST", data, this);
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ������ ���������� ���������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        private void executeCommand()
        {
            mWait = false; //����� ��� �������� �������
            if (mDeviceID == 0)
            {
                //��� �����������, ������������� �� �������
                deviceAuthorization();
                return;
            }

            if (mPool.Count > 0 && !mWebClient.IsBusy)
            {
                AWebQuery cmd = mPool[0];
                mPool.Remove(cmd);
                cmd.send(mWebClient, mDeviceID);
            }
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ���������� �������� �� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void evCommandCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            AWebQuery cmd = e.UserState as AWebQuery;
            if (cmd == null && e.UserState is ANetworkWeb)
            {
                mLoginAth = false;
                if (e.Error != null)
                {
                    waitExecute();
                    return;
                }
                AWebParameters param = new AWebParameters(e.Result);
                mDeviceID = param.keyInteger("deviceID", mDeviceID);
                if (mDeviceID == 0)
                {
                    waitExecute();
                    return;
                }
                executeCommand();
                return;
            }

            if (e.Error != null)
            {
                if (cmd.sendQueue() < 10)
                {
                    mPool.Insert(0, cmd);
                }
                waitExecute();
                return;
            }
            cmd.execute(e.Result);
            executeCommand();
        }
        ///--------------------------------------------------------------------------------------





         ///=====================================================================================
        ///
        /// <summary>
        /// ����, ����� ����� ��������� ������� �������� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        protected void waitExecute()
        {
            mWait = true;
            mTimeWait = TimeSpan.Zero;
        }
        ///--------------------------------------------------------------------------------------






         ///=====================================================================================
        ///
        /// <summary>
        /// ����� ��� ��������� ������� ������
        /// </summary>
        /// 
        ///--------------------------------------------------------------------------------------
        public void update(TimeSpan gameTime)
        {
            if (mWait)
            {
                mTimeWait += gameTime;
                if (mTimeWait.TotalMilliseconds > cTimeOut)
                {
                    executeCommand();
                }
            }
        }
        ///--------------------------------------------------------------------------------------








    }
}
