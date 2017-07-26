using System;

namespace NewCardUIDToParsec
{
    /// <summary>
    /// Базовый класс считывателя
    /// </summary>
    abstract class EnumCardsBase : IEnumCards
    {
        #region Базовая реализация методов

        /// <summary>
        /// Старт работы считывателя
        /// </summary>
        protected abstract void DoStart();

        /// <summary>
        /// Удаление объекта считывателя
        /// </summary>
        protected abstract void DoDispose();

        /// <summary>
        /// Генерация события считывателя
        /// </summary>
        /// <param name="cardNum">UID пропуска</param>
        /// <param name="inserted">пропуск поднесен/убран</param>
        protected virtual void OnCardNotify(string cardNum, bool inserted)
        {
            if (CardNotify != null)
                CardNotify(this, new EnumCardsNotifyEventArgs(cardNum, inserted));
        }

        #endregion Базовая реализация методов

        #region IEnumCards

        public event EventHandler<EnumCardsNotifyEventArgs> CardNotify;

        public void Start()
        {
            DoStart();
        }

        #endregion IEnumCards

        #region IDisposable

        public void Dispose()
        {
            DoDispose();
        }

        #endregion IDisposable
    }
}
