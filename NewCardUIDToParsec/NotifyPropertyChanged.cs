using System.Collections.Generic;
using System.ComponentModel;

namespace NewCardUIDToParsec
{
    /// <summary>
    /// Вспомогательный класс для установки свойств с генерацией события PropertyChanged
    /// </summary>
    static class NotifyPropertyChanged
    {
        /// <summary>
        /// Установки свойства с генерацией события PropertyChanged
        /// </summary>
        /// <typeparam name="T">тип свойства</typeparam>
        /// <param name="obj">объект, для которого устанавливается значение свойста</param>
        /// <param name="propertyChanged">обработчик события PropertyChanged</param>
        /// <param name="field">поле, соответствующее свойству</param>
        /// <param name="value">устанавливаемое значение</param>
        /// <param name="propertyName">имя свойства</param>
        static public void SetField<T>(INotifyPropertyChanged obj, PropertyChangedEventHandler propertyChanged, ref T field, T value, string propertyName)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                if (propertyChanged != null)
                    propertyChanged(obj, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
