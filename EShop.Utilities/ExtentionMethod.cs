using System.Globalization;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EShop.Utilities
{
    public static class ExtentionMethod
    {
        public static string ToCultureDate(this DateTime date, bool ShortDate = false)
        {
            try
            {
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Contains("en"))
                {
                    if (!ShortDate)
                        return String.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}", date.Year, date.Month, date.Day, date.Hour, date.Minute).ToEnglishChar();

                    return String.Format("{0:0000}/{1:00}/{2:00}", date.Year, date.Month, date.Day).ToEnglishChar();
                }

                PersianCalendar pc = new PersianCalendar();

                if (!ShortDate)
                    return String.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}", pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date), date.Hour, date.Minute, date.Second);
                else
                    return String.Format("{0:0000}/{1:00}/{2:00}", pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date));

            }
            catch
            {
                return "";
            }
        }
        public static string ToCultureDateHour(this DateTime date)
        {
            try
            {
                if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Contains("en"))
                    return String.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}", date.Year, date.Month, date.Day, date.Hour, date.Minute).ToEnglishChar();

                PersianCalendar pc = new PersianCalendar();
                return String.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}", pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date), date.Hour, date.Minute);
            }
            catch
            {
                return "";
            }
        }

        public static DateTime ToMiladiDateSpecial(this string date, int Interval, bool AddEnd)
        {
            try
            {
                if (date.Trim() == string.Empty)
                    return new DateTime();
                if (Interval == 1 || Interval == 2)
                {
                    return date.ToMiladiDate();
                }
                else if (Interval == 3 || Interval == 4)
                {
                    Dictionary<string, int> MonthPersina = new Dictionary<string, int>();
                    string[] str = date.Split(new char[] { ' ' });
                    int Year = int.Parse(str[0]);
                    int Month = GetPersinMonthDictionary()[str[1]];
                    if (!AddEnd)
                    {
                        string _datestr = string.Format("{0}/{1}/{2} {3}:{4}:{5}", Year, Month, 1, 0, 0, 0);
                        return _datestr.ToMiladiDate();
                    }
                    else
                    {
                        string _datestr = string.Format("{0}/{1}/{2} {3}:{4}:{5}", Year, Month, PersianMonthDays(Month, Year), 23, 59, 59);
                        return _datestr.ToMiladiDate();
                    }
                }
                else if (Interval == 5)
                {
                    int Year = int.Parse(date);
                    if (!AddEnd)
                    {
                        string _datestr = string.Format("{0}/{1}/{2} {3}:{4}:{5}", Year, 1, 1, 0, 0, 0);
                        return _datestr.ToMiladiDate();
                    }
                    else
                    {
                        string _datestr = string.Format("{0}/{1}/{2} {3}:{4}:{5}", Year, 12, PersianMonthDays(12, Year), 23, 59, 59);
                        return _datestr.ToMiladiDate();
                    }
                }
            }
            catch
            {

            }
            return new DateTime();
        }

        public static Dictionary<string, int> GetPersinMonthDictionary()
        {
            Dictionary<string, int> m = new Dictionary<string, int>();
            m.Add("فروردين", 1);
            m.Add("ارديبهشت", 2);
            m.Add("خرداد", 3);
            m.Add("تير", 4);
            m.Add("مرداد", 5);
            m.Add("شهريور", 6);
            m.Add("مهر", 7);
            m.Add("آبان", 8);
            m.Add("آذر", 9);
            m.Add("دي", 10);
            m.Add("بهمن", 11);
            m.Add("اسفند", 12);
            return m;
        }
        public static int ToPersianYear(this DateTime date)
        {
            try
            {
                PersianCalendar pc = new PersianCalendar();
                return pc.GetYear(date);
            }
            catch
            {
                return 0;
            }
        }
        public static int ToPersianMonth(this DateTime date)
        {
            try
            {
                PersianCalendar pc = new PersianCalendar();
                return pc.GetMonth(date);
            }
            catch
            {
                return 0;
            }
        }
        public static int PersianMonthDays(this int month, int Year)
        {
            try
            {
                PersianCalendar pc = new PersianCalendar();
                if (month < 7) return 31;
                else if (month < 12) return 30;
                else if (pc.IsLeapYear(Year)) return 30;
                else return 29;
            }
            catch
            {
                return 0;
            }
        }

        public static string ToCultureDate(this DateTime? datetime, bool Short = false)
        {
            try
            {
                if (datetime == null)
                    return "-";
                else
                {
                    DateTime date = (DateTime)datetime;

                    if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "en")
                    {
                        if (!Short)
                            return String.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}", date.Year, date.Month, date.Day, date.Hour, date.Minute).ToEnglishChar();

                        return String.Format("{0:0000}/{1:00}/{2:00}", date.Year, date.Month, date.Day).ToEnglishChar();
                    }

                    PersianCalendar pc = new PersianCalendar();

                    if (!Short)
                        return String.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}", pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date), date.Hour, date.Minute, date.Second);
                    else
                        return String.Format("{0:0000}/{1:00}/{2:00}", pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date));
                }
            }
            catch
            {
                return "";
            }
        }

        public static DateTime ToMiladiDate(this string date)
        {
            try
            {
                if (!string.IsNullOrEmpty(date))
                {
                    string[] mdate = date.Split(new char[] { '/', '-', ',', '_', ' ', '.', ':' });
                    if (mdate.Length < 3)
                        return new DateTime();
                    int year = Convert.ToInt32(mdate[0]);
                    int mon = Convert.ToInt32(mdate[1]);
                    int day = Convert.ToInt32(mdate[2]);
                    int hour = 0;
                    if (mdate.Length > 3)
                        hour = Convert.ToInt32(mdate[3]);
                    int min = 0;
                    if (mdate.Length > 4)
                        min = Convert.ToInt32(mdate[4]);

                    int sec = 0;
                    if (mdate.Length > 5)
                        sec = Convert.ToInt32(mdate[5]);

                    PersianCalendar pc = new PersianCalendar();
                    // DateTime dt = new DateTime(year, mon, day, new PersianCalendar());
                    if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "en")
                        return new DateTime(year, mon, day, hour, min, sec);

                    return pc.ToDateTime(year, mon, day, hour, min, sec, 0);
                }
                return new DateTime();
            }
            catch (Exception)
            {
                return new DateTime();
            }
        }


        public static string ToEnglishChar(this string text)
        {
            string str = text.Replace("۰", "0") // 0
            .Replace("۱", "1") // 1
            .Replace("۲", "2") // 2
            .Replace("۳", "3") // 3
            .Replace("۴", "4") // 4
            .Replace("۵", "5") // 5
            .Replace("۶", "6") // 6
            .Replace("۷", "7") // 7
            .Replace("۸", "8") // 8
            .Replace("۹", "9"); // 9

            return str;
        }

        public static string ToSplitString(this long[] items)
        {
            return String.Join(",", items);
        }
        public static string ToSplitString(this List<string> items)
        {
            return String.Join(",", items);
        }
        public static string ToSplitString(this string[] items)
        {
            return String.Join(",", items);
        }

        //public static DateTime GetForstDayOfYearByCulture(this DateTime date)
        //{
        //    if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "en")
        //    {
        //        return new DateTime(date.Year, 1, 1);
        //    }
        //    else
        //    {

        //        PersianCalendar pc = new PersianCalendar();
        //        var dt = String.Format("{0:0000}/{1:00}/{2:00}", pc.GetYear(date), 1, 1);
        //        return dt.ToMiladiDate();
        //    }
        //}

        public static bool IsValidEmail(this string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static XElement ToXElement<T>(this object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
                }
            }
        }
    }
}