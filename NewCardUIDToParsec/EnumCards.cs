using System;
using System.Runtime.InteropServices;
using System.Threading;
using ZPort;
using ZREADER;

namespace NewCardUIDToParsec
{
    /// <summary>
    /// Реальный считыватель Z-2
    /// </summary>
    class EnumCards : EnumCardsBase
    {
        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="readerPortName">имя порта, например COM3</param>
        /// <param name="openPortDelay">задержка открытия порта, мсек</param>
        public EnumCards(string readerPortName, int openPortDelay = 0)
        {
            this.readerPortName = readerPortName;
            this.openPortDelay = openPortDelay;
        }

        #endregion Конструктор

        #region Override

        /// <summary>
        /// Запуск
        /// </summary>
        protected override void DoStart()
        {
            bool notifyThreadStarted = false;

            int hr;
            hr = ZRIntf.ZR_Initialize(ZPIntf.ZP_IF_NO_MSG_LOOP);
            if (hr < 0)
                throw new ApplicationException(string.Format("Ошибка ZR_Initialize ({0}).", hr));

            try
            {
                // открытие считывателя
                OpenReader();

                hr = ZRIntf.ZR_Rd_SearchCards(m_hRd);
                if (hr < 0)
                    throw new ApplicationException(string.Format("Ошибка ZR_Rd_SearchCards ({0}).", hr));

                // считывание номера карты, приложенной в момент запуска приложения
                ZR_CARD_INFO rInfo = new ZR_CARD_INFO();
                hr = ZRIntf.ZR_Rd_FindNextCard(m_hRd, ref rInfo);
                if (hr < 0)
                    throw new ApplicationException(string.Format("Ошибка ZR_Rd_FindNextCard ({0}).", hr));
                else
                    OnCardNotify(ZRIntf.CardNumToStr(rInfo.nNum, rInfo.nType), true);

                ZRIntf.ZR_Rd_FindNextCard(m_hRd);

                m_oEvent = new ManualResetEvent(false);
                ZR_RD_NOTIFY_SETTINGS rNS = new ZR_RD_NOTIFY_SETTINGS(ZRIntf.ZR_RNF_EXIST_CARD, m_oEvent.SafeWaitHandle);
                hr = ZRIntf.ZR_Rd_SetNotification(m_hRd, rNS);
                if (hr < 0)
                    throw new ApplicationException(string.Format("Ошибка ZR_Rd_SetNotification ({0}).", hr));

                StartNotifyThread();

                notifyThreadStarted = true;
            }
            finally
            {
                if (!notifyThreadStarted)
                    CloseReader();
            }
        }

        protected override void DoDispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Override

        #region Private

        /// <summary>
        /// Открытие считывателя
        /// </summary>
        private void OpenReader()
        {
            int hr;
            #region Открытие порта считывателя
            //ZR_RD_OPEN_PARAMS rOpen = new ZR_RD_OPEN_PARAMS(ZP_PORT_TYPE.ZP_PORT_COM, readerPortName);
            //ZR_RD_INFO rRdInf = new ZR_RD_INFO();
            //hr = ZRIntf.ZR_Rd_Open(ref m_hRd, ref rOpen, ref rRdInf);
            //if (hr < 0)
            //    throw new ApplicationException(string.Format("Ошибка ZR_Rd_Open ({0}).", hr));
            #endregion Открытие порта считывателя

            #region Открытие порта считывателя через универсальный порт (через ZP_Open)
            IntPtr portHandle = IntPtr.Zero;
            ZP_PORT_OPEN_PARAMS portOpenParams = new ZP_PORT_OPEN_PARAMS
            {
                // Имя порта
                szName = readerPortName,
                // Тип порта
                nType = ZP_PORT_TYPE.ZP_PORT_COM
            };

            if (openPortDelay <= 0)
                // если задержка открытия порта не установлена, пытаемся его открыть
                hr = ZPIntf.ZP_Port_Open(ref portHandle, ref portOpenParams);
            else
            {
                // пытаемся открыть порт, пока не получится или пока не пройдет время задержки открытия порта openPortDelay
                hr = ZPIntf.E_ACCESSDENIED;
                while (hr != 0 && openPortDelay > 0)
                {
                    const int delay = 100;
                    Thread.Sleep(delay);
                    openPortDelay -= delay;

                    hr = ZPIntf.ZP_Port_Open(ref portHandle, ref portOpenParams);
                }
            }
            if (hr != 0)
                throw new ApplicationException(string.Format("Ошибка ZP_Port_Open ({0}).", hr));

            ZR_RD_OPEN_PARAMS rOpen = new ZR_RD_OPEN_PARAMS(0, null, portHandle);
            ZR_RD_INFO rRdInf = new ZR_RD_INFO();
            hr = ZRIntf.ZR_Rd_Open(ref m_hRd, ref rOpen, ref rRdInf);
            if (hr < 0)
                throw new ApplicationException(string.Format("Ошибка ZR_Rd_Open ({0}).", hr));
            #endregion Открытие порта считывателя через универсальный порт (через ZP_Open)
        }

        /// <summary>
        /// Закрытие считывателя
        /// </summary>
        private static void CloseReader()
        {
            StopNotifyThread();
            if (m_hRd != IntPtr.Zero)
            {
                ZRIntf.ZR_CloseHandle(m_hRd);
            }
            ZRIntf.ZR_Finalyze();
        }

        /// <summary>
        /// Обработка события считывателя
        /// </summary>
        /// <returns></returns>
        int CheckNotifyMsgs()
        {
            int hr;
            UInt32 nMsg = 0;
            IntPtr nMsgParam = IntPtr.Zero;
            while ((hr = ZRIntf.ZR_Rd_GetNextMessage(m_hRd, ref nMsg, ref nMsgParam)) == ZRIntf.S_OK)
            {
                switch (nMsg)
                {
                    case ZRIntf.ZR_RN_CARD_INSERT:
                        {
                            ZR_CARD_INFO pInfo = (ZR_CARD_INFO)Marshal.PtrToStructure(nMsgParam, typeof(ZR_CARD_INFO));
                            OnCardNotify(ZRIntf.CardNumToStr(pInfo.nNum, pInfo.nType), true);
                        }
                        break;
                    case ZRIntf.ZR_RN_CARD_REMOVE:
                        OnCardNotify("", false);
                        break;
                }
            }
            if (hr == ZPIntf.ZP_S_NOTFOUND)
                hr = ZRIntf.S_OK;
            return hr;
        }

        /// <summary>
        /// Обработка событий считывателя
        /// </summary>
        /// <returns></returns>
        void DoNotifyWork()
        {
            while (m_fThreadActive)
            {
                if (m_oEvent.WaitOne())
                {
                    m_oEvent.Reset();
                    if (m_hRd != IntPtr.Zero)
                        CheckNotifyMsgs();
                }
            }
        }

        /// <summary>
        /// Запуск потока обработки событий считывателя
        /// </summary>
        void StartNotifyThread()
        {
            if (m_oThread != null)
                return;
            m_fThreadActive = true;
            m_oThread = new Thread(DoNotifyWork);
            m_oThread.Start();
        }

        /// <summary>
        /// Останов потока обработки событий считывателя
        /// </summary>
        static void StopNotifyThread()
        {
            if (m_oThread == null)
                return;
            m_fThreadActive = false;
            m_oEvent.Set();
            m_oThread.Join();
            m_oThread = null;
        }

        #endregion Private

        #region Финализация
        // см. https://msdn.microsoft.com/ru-ru/library/b1yfkh5e(v=vs.110).aspx

        private bool disposed = false;

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free other state (managed objects).
                }
                CloseReader();
                disposed = true;
            }
        }

        ~EnumCards()
        {
            Dispose(false);
        }

        #endregion Финализация

        #region Внутренние поля

        /// <summary>
        /// Имя порта считывателя
        /// </summary>
        string readerPortName;

        /// <summary>
        /// Задержка открытия порта считывателя, мсек
        /// </summary>
        int openPortDelay;

        #endregion Внутренние поля

        #region Поля для работы с SDK

        /// <summary>
        /// Указатель на считыватель
        /// </summary>
        static IntPtr m_hRd = IntPtr.Zero;

        static ManualResetEvent m_oEvent = null;
        static bool m_fThreadActive;
        static Thread m_oThread = null;

        #endregion Поля для работы с SDK
    }
}
