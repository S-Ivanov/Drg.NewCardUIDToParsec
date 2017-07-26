using System.Timers;

namespace NewCardUIDToParsec
{
    /// <summary>
    /// Эмулятор считывателя
    /// </summary>
    class EnumCards_Mock : EnumCardsBase
    {
        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="cardNum">UID пропуска</param>
        /// <param name="interval">время цикла "пропуск поднесен/убран", мсек</param>
        public EnumCards_Mock(string cardNum, double interval)
        {
            this.cardNum = cardNum;
            timer = new Timer(interval);
            timer.Elapsed += Timer_Elapsed;
        }

        #endregion Конструктор

        #region Private

        /// <summary>
        /// Обработчик событий таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            OnCardNotify(cardNum, inserted);
            inserted = !inserted;
        }

        #endregion Private

        #region Override

        protected override void DoStart()
        {
            timer.Start();
        }

        protected override void OnCardNotify(string cardNum, bool inserted)
        {
            base.OnCardNotify(cardNum, inserted);
            inserted = !inserted;
        }

        protected override void DoDispose()
        {
        }

        #endregion Override

        #region Внутренние поля

        /// <summary>
        /// Таймер для имитации цикла "пропуск поднесен/убран"
        /// </summary>
        Timer timer;

        /// <summary>
        /// UID пропуска
        /// </summary>
        string cardNum;

        /// <summary>
        /// Пропуск поднесен/убран
        /// </summary>
        bool inserted = false;

        #endregion Внутренние поля
    }
}
