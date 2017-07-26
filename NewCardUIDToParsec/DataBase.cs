using System;
using System.Linq;

namespace NewCardUIDToParsec
{
    /// <summary>
    /// База данных Parsec
    /// </summary>
    class DataBase : IDataBase
    {
        #region IDataBase

        public PersonInfo ReadByCardNum(string cardNum)
        {
            try
            {
                using (ParsecDBDataContext parsecDBDataContext = new ParsecDBDataContext())
                {
                    var personInfo = parsecDBDataContext.v_DRG_CARD_IDENTIFIERS
                        .Where(r => r.CARD_UID == cardNum)
                        .Select(r => new PersonInfo
                        {
                            FirstName = r.FIRST_NAME,
                            LastName = r.LAST_NAME,
                            MiddleName = r.MIDDLE_NAME,
                            Pers_ID = r.PERS_ID,
                            TabNum = r.TAB,
                            Department = r.ORG_NAME,
                            Post = r.POST.ToString()
                        })
                        .FirstOrDefault();
                    ReadPhoto(parsecDBDataContext, personInfo);
                    return personInfo;
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("Ошибка ReadByCardNum: " + e.Message);
            }
        }

        private static void ReadPhoto(ParsecDBDataContext parsecDBDataContext, PersonInfo personInfo)
        {
            if (personInfo != null)
            {
                try
                {
                    var data = parsecDBDataContext.Person_GetPhoto(personInfo.Pers_ID).FirstOrDefault();
                    if (data != null)
                        personInfo.Photo = data.PHOTO.ToArray();
                }
                catch (Exception e)
                {
                    throw new ApplicationException("Ошибка ReadPhoto: " + e.Message);
                }
            }
        }

        public PersonInfo ReadByTabNum(string tabNum)
        {
            try
            {
                using (ParsecDBDataContext parsecDBDataContext = new ParsecDBDataContext())
                {
                    var persons = parsecDBDataContext.v_DRG_CARD_IDENTIFIERS.Where(r => r.TAB == tabNum);
                    var personCount = persons.Count();
                    if (personCount == 0)
                        return null;
                    else if (personCount == 1)
                    {
                        var personInfo = persons
                            .Select(r => new PersonInfo
                            {
                                FirstName = r.FIRST_NAME,
                                LastName = r.LAST_NAME,
                                MiddleName = r.MIDDLE_NAME,
                                Pers_ID = r.PERS_ID,
                                TabNum = tabNum,
                                Department = r.ORG_NAME,
                                Post = r.POST.ToString()
                            })
                            .First();
                        ReadPhoto(parsecDBDataContext, personInfo);
                        return personInfo;
                    }
                    else
                        throw new TabNumDuplicatedException(tabNum);
                }
            }
            catch (TabNumDuplicatedException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Ошибка ReadByTabNum: " + e.Message);
            }
        }

        public void ClearCardNum(Guid persID)
        {
            try
            {
                using (ParsecDBDataContext parsecDBDataContext = new ParsecDBDataContext())
                {
                    var record = parsecDBDataContext.DRG_CARD_UID.Where(r => r.PERS_ID == persID).FirstOrDefault();
                    if (record != null)
                    {
                        parsecDBDataContext.DRG_CARD_UID.DeleteOnSubmit(record);
                        parsecDBDataContext.SubmitChanges();
                    }
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("Ошибка ClearCardNum: " + e.Message);
            }
        }

        public void SaveCardNum(Guid persID, string cardNum)
        {
            try
            {
                using (ParsecDBDataContext parsecDBDataContext = new ParsecDBDataContext())
                {
                    var record = parsecDBDataContext.DRG_CARD_UID.Where(r => r.PERS_ID == persID).FirstOrDefault();
                    if (record == null)
                    {
                        var newRecord = new DRG_CARD_UID { PERS_ID = persID, UID = cardNum };
                        parsecDBDataContext.DRG_CARD_UID.InsertOnSubmit(newRecord);
                    }
                    else
                    {
                        record.UID = cardNum;
                    }
                    parsecDBDataContext.SubmitChanges();
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("Ошибка SaveCardNum: " + e.Message);
            }
        }

        #endregion IDataBase
    }
}
