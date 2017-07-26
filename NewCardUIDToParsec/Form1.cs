//#define USE_ENUMCARDS_MOCK

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

#if USE_ENUMCARDS_MOCK
    //
#else
    using System.Configuration;
#endif 

namespace NewCardUIDToParsec
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // создание читателя карт 
                IEnumCards enumCards;
#if USE_ENUMCARDS_MOCK
                string cardNum = "E8E42F4E";
                double interval = 10 * 1000;
                enumCards = new EnumCards_Mock(cardNum, interval);
#else
                enumCards = new EnumCards(ConfigurationManager.AppSettings["readerPort"], int.Parse(ConfigurationManager.AppSettings["openPortDelay"]));
#endif 
                // создание объекта бизнес-логики 
                businessLogic = new BusinessLogic(enumCards, new DataBase(), WindowsFormsSynchronizationContext.Current);

                // настройка биндингов:
                // поля вывода
                tbCardNum.DataBindings.Add("Text", businessLogic, "CardNum");
                tbDepartment.DataBindings.Add("Text", businessLogic, "PersonInfo.Department");
                tbPost.DataBindings.Add("Text", businessLogic, "PersonInfo.Post");
                tbFirstName.DataBindings.Add("Text", businessLogic, "PersonInfo.FirstName");
                tbLastName.DataBindings.Add("Text", businessLogic, "PersonInfo.LastName");
                tbMiddleName.DataBindings.Add("Text", businessLogic, "PersonInfo.MiddleName");
                // поле ввода табельного номера
                tbTabNum.DataBindings.Add("Text", businessLogic, "PersonInfo.TabNum", true, DataSourceUpdateMode.OnPropertyChanged);
                tbTabNum.DataBindings.Add("ReadOnly", businessLogic, "СardProcessed");
                // кнопки
                btnCheck.DataBindings.Add("Enabled", businessLogic, "CanCheck");
                btnClear.DataBindings.Add("Enabled", businessLogic, "CanClear");

                // настройка отображения текста в строке статуса
                businessLogic.PropertyChanged += BusinessLogic_PropertyChanged;
            }
            catch (Exception ex)
            {
                Program.ShowErrorMessage(ex.Message);
                this.Close();
            }
        }

        /// <summary>
        /// Обработчик изменения свойств объекта бизнес-логики
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Большинство изменений свойств объекта бизнес-логики обрабатываются через DataBinding - см. настройки в методе Form1_Load
        /// Здесь обрабатываются только те события, для которых нельзя установить биндинг
        /// </remarks>
        private void BusinessLogic_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "StatusText")
                tslStatus.Text = businessLogic.StatusText;
            else if (e.PropertyName == "PersonInfo")
            {
                if (businessLogic.PersonInfo.Photo == null)
                    pbPhoto.Image = null;
                else
                {
                    var stream = new MemoryStream(businessLogic.PersonInfo.Photo);
                    pbPhoto.Image = Image.FromStream(stream);
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (businessLogic != null)
            {
                businessLogic.PropertyChanged -= BusinessLogic_PropertyChanged;
                businessLogic.Dispose();
            }
        }

        /// <summary>
        /// Нажатие кнопки "Проверить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                businessLogic.CheckTabNum();
            }
            catch (TabNumDuplicatedException ex)
            {
                Program.ShowErrorMessage(string.Format("Найдены дубли табельного номера {0}.", ex.TabNum));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Нажатие кнопки "Очистить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            businessLogic.ClearCardBinding();
        }

        /// <summary>
        /// Объект бизнес-логики
        /// </summary>
        BusinessLogic businessLogic;
    }
}
