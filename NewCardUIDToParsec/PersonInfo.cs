using System;
using System.ComponentModel;

namespace NewCardUIDToParsec
{
    /// <summary>
    /// Информация о работнике
    /// </summary>
    class PersonInfo : INotifyPropertyChanged
    {
        #region Свойства

        /// <summary>
        /// Табельный номер
        /// </summary>
        public string TabNum
        {
            get { return tabNum; }
            set { NotifyPropertyChanged.SetField(this, PropertyChanged, ref tabNum, value, "TabNum"); }
        }
        string tabNum;

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName
        {
            get { return firstName; }
            set { NotifyPropertyChanged.SetField(this, PropertyChanged, ref firstName, value, "FirstName"); }
        }
        string firstName;

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName
        {
            get { return lastName; }
            set { NotifyPropertyChanged.SetField(this, PropertyChanged, ref lastName, value, "LastName"); }
        }
        string lastName;

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName
        {
            get { return middleName; }
            set { NotifyPropertyChanged.SetField(this, PropertyChanged, ref middleName, value, "MiddleName"); }
        }
        string middleName;

        /// <summary>
        /// Подразделение
        /// </summary>
        public string Department
        {
            get { return department; }
            set { NotifyPropertyChanged.SetField(this, PropertyChanged, ref department, value, "Department"); }
        }
        string department;

        /// <summary>
        /// Должность
        /// </summary>
        public string Post
        {
            get { return post; }
            set { NotifyPropertyChanged.SetField(this, PropertyChanged, ref post, value, "Post"); }
        }
        string post;

        /// <summary>
        /// Фотография
        /// </summary>
        public byte[] Photo
        {
            get { return photo; }
            set { NotifyPropertyChanged.SetField(this, PropertyChanged, ref photo, value, "Photo"); }
        }
        byte[] photo;

        /// <summary>
        /// Идентификатор работника
        /// </summary>
        public Guid Pers_ID
        {
            get { return pers_ID; }
            set { NotifyPropertyChanged.SetField(this, PropertyChanged, ref pers_ID, value, "Pers_ID"); }
        }
        Guid pers_ID;

        #endregion Свойства

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
    
        #endregion INotifyPropertyChanged
    }
}
