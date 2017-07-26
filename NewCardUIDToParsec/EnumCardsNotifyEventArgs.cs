using System;

namespace NewCardUIDToParsec
{
    /// <summary>
    /// Событие считывателя
    /// </summary>
    public class EnumCardsNotifyEventArgs : EventArgs
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="cardNum">UID пропуска</param>
        /// <param name="inserted">пропуск поднесен/убран</param>
        public EnumCardsNotifyEventArgs(string cardNum, bool inserted)
        {
            CardNum = cardNum;
            Inserted = inserted;
        }

        /// <summary>
        /// UID пропуска
        /// </summary>
        public string CardNum { get; private set; }

        /// <summary>
        /// Пропуск поднесен/убран
        /// </summary>
        public bool Inserted { get; private set; }
    }
}
