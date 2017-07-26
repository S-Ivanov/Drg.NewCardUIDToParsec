using System;

namespace NewCardUIDToParsec
{
    /// <summary>
    /// Считыватель карт
    /// </summary>
    interface IEnumCards : IDisposable
    {
        /// <summary>
        /// Событие карта поднесена/убрана
        /// </summary>
        event EventHandler<EnumCardsNotifyEventArgs> CardNotify;

        /// <summary>
        /// Запуск
        /// </summary>
        void Start();
    }
}