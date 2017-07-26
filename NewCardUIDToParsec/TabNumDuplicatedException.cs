using System;

namespace NewCardUIDToParsec
{
    /// <summary>
    /// Ошибка дублирования табельного номера
    /// </summary>
    class TabNumDuplicatedException : ApplicationException
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="tabNum">табельный номер</param>
        public TabNumDuplicatedException(string tabNum)
        {
            TabNum = tabNum;
        }

        /// <summary>
        /// Табельный номер
        /// </summary>
        public string TabNum { get; private set; }
    }
}
