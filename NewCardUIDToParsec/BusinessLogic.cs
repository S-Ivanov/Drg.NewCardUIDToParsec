using System;
using System.ComponentModel;
using System.Threading;

namespace NewCardUIDToParsec
{
    /// <summary>
    /// Бизнес-логика
    /// </summary>
    class BusinessLogic : INotifyPropertyChanged, IDisposable
    {
        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="enumCards">считыватель карт</param>
        /// <param name="database">база данных</param>
        /// <param name="synchronizationContext">контекст синхронизации</param>
        public BusinessLogic(IEnumCards enumCards, IDataBase database, SynchronizationContext synchronizationContext = null)
        {
            if (enumCards == null || database == null)
                throw new ArgumentNullException();

            this.enumCards = enumCards;
            this.database = database;
            this.synchronizationContext = synchronizationContext;

            try
            {
                enumCards.CardNotify += EnumCards_CardNotify;
                enumCards.Start();
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        #endregion Конструктор

        #region Свойства

        /// <summary>
        /// Номер карточки
        /// </summary>
        public string CardNum
        {
            get { return cardNum; }
            private set { NotifyPropertyChanged.SetField(this, PropertyChanged, ref cardNum, value, "CardNum"); }
        }
        string cardNum;

        /// <summary>
        /// Информация о работнике
        /// </summary>
        public PersonInfo PersonInfo
        {
            get { return personInfo; }
            private set
            {
                personInfo.PropertyChanged -= PersonInfo_PropertyChanged;
                NotifyPropertyChanged.SetField(this, PropertyChanged, ref personInfo, value, "PersonInfo");
                personInfo.PropertyChanged += PersonInfo_PropertyChanged;
            }
        }
        PersonInfo personInfo = new PersonInfo();

        /// <summary>
        /// Номер карточки
        /// </summary>
        public string StatusText
        {
            get { return statusText; }
            private set { NotifyPropertyChanged.SetField(this, PropertyChanged, ref statusText, value, "StatusText"); }
        }
        string statusText;

        /// <summary>
        /// Можно ли проверять данные
        /// </summary>
        public bool CanCheck
        {
            get { return canCheck; }
            private set { NotifyPropertyChanged.SetField(this, PropertyChanged, ref canCheck, value, "CanCheck"); }
        }
        bool canCheck;

        /// <summary>
        /// Можно ли очищать данные
        /// </summary>
        public bool CanClear
        {
            get { return canClear; }
            private set { NotifyPropertyChanged.SetField(this, PropertyChanged, ref canClear, value, "CanClear"); }
        }
        bool canClear;

        /// <summary>
        /// Индикатор того, что поднесенный пропуск уже обработан
        /// </summary>
        public bool СardProcessed
        {
            get { return cardProcessed; }
            private set { NotifyPropertyChanged.SetField(this, PropertyChanged, ref cardProcessed, value, "СardProcessed"); }
        }
        bool cardProcessed = true;

        #endregion Свойства

        #region Публичные методы

        /// <summary>
        /// Проверка наличия табельного номера
        /// </summary>
        public void CheckTabNum()
        {
            var personInfo = database.ReadByTabNum(PersonInfo.TabNum);
            if (personInfo == null)
                StatusText = "Табельный номер не найден";
            else
            {
                PersonInfo = personInfo;
                StatusText = "Табельный номер найден";
            }
            CanCheck = false;
        }

        /// <summary>
        /// Очистка привязки пропуска
        /// </summary>
        public void ClearCardBinding()
        {
            if (PersonInfo.Pers_ID != Guid.Empty)
            {
                database.ClearCardNum(PersonInfo.Pers_ID);
                PersonInfo = new PersonInfo();
                CanClear = СardProcessed = false;
                StatusText = "Привязка пропуска очищена";
            }
        }

        #endregion Публичные методы

        #region Внутренние методы

        /// <summary>
        /// Обработка изменения табельного номера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PersonInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TabNum")
            {
                CanCheck = !string.IsNullOrEmpty(personInfo.TabNum);
                if (personInfo.Pers_ID != Guid.Empty)
                    PersonInfo = new PersonInfo { TabNum = personInfo.TabNum };
                StatusText = CanCheck ? "Готов к проверке" : string.Empty;
            }
        }

        /// <summary>
        /// Обработка события считывателя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnumCards_CardNotify(object sender, EnumCardsNotifyEventArgs e)
        {
            if (synchronizationContext == null)
            {
                if (e.Inserted)
                    CardInserted(e.CardNum);
                else
                    CardRemoved();
            }
            else
            {
                synchronizationContext.Post(
                    o =>
                    {
                        if (e.Inserted)
                            CardInserted(e.CardNum);
                        else
                            CardRemoved();
                    },
                    null
                );
            }
        }

        /// <summary>
        /// Карта вынута
        /// </summary>
        private void CardRemoved()
        {
            var pers_ID = PersonInfo.Pers_ID;
            PersonInfo = new PersonInfo();
            if (pers_ID == Guid.Empty)
                StatusText = "Обработка пропуска не выполнена";
            else if (СardProcessed)
                StatusText = "Пропуск убран";
            else // if (!СardProcessed)
            {
                // сохраняем только если если человек не опознан по номеру пропуска
                database.SaveCardNum(pers_ID, CardNum);
                StatusText = "Сохранено";
            }

            CardNum = string.Empty;
            CanClear = false;
            СardProcessed = true;
        }

        /// <summary>
        /// Карта поднесена
        /// </summary>
        /// <param name="cardNum"></param>
        private void CardInserted(string cardNum)
        {
            CardNum = cardNum;

            if (cardNum == string.Empty)
                StatusText = "Готов";
            else
            {
                var personInfo = database.ReadByCardNum(cardNum);
                if (personInfo == null)
                {
                    PersonInfo = new PersonInfo();
                    StatusText = "Пропуск поднесен";
                    СardProcessed = false;
                }
                else
                {
                    PersonInfo = personInfo;
                    StatusText = "Пропуск уже обработан";
                    СardProcessed = CanClear = true;
                }
            }
        }

        #endregion Внутренние методы

        #region Поля

        /// <summary>
        /// Считыватель карт
        /// </summary>
        IEnumCards enumCards = null;

        /// <summary>
        /// База данных
        /// </summary>
        IDataBase database = null;

        /// <summary>
        /// Контекст синхронизации
        /// </summary>
        SynchronizationContext synchronizationContext;

        #endregion Поля

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged

        #region IDisposable

        public void Dispose()
        {
            personInfo.PropertyChanged -= PersonInfo_PropertyChanged;
            if (enumCards != null)
            {
                enumCards.CardNotify -= EnumCards_CardNotify;
                enumCards.Dispose();
            }
        }

        #endregion IDisposable
    }
}
