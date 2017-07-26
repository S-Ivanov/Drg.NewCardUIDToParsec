using System;

namespace NewCardUIDToParsec
{
    /// <summary>
    /// База данных
    /// </summary>
    interface IDataBase
    {
        /// <summary>
        /// Читать информацию о работнике по номеру пропуска
        /// </summary>
        /// <param name="cardNum">номер пропуска</param>
        /// <returns></returns>
        PersonInfo ReadByCardNum(string cardNum);

        /// <summary>
        /// Читать информацию о работнике по табельному номеру
        /// </summary>
        /// <param name="tabNum">табельный номер</param>
        /// <returns></returns>
        PersonInfo ReadByTabNum(string tabNum);

        /// <summary>
        /// Очистить привязку пропуска
        /// </summary>
        /// <param name="persID">идентификатор работника</param>
        void ClearCardNum(Guid persID);

        /// <summary>
        /// Сохранить номер пропуска
        /// </summary>
        /// <param name="persID">идентификатор работника</param>
        /// <param name="cardNum">номер пропуска</param>
        void SaveCardNum(Guid persID, string cardNum);
    }
}
