using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
namespace AVA.UI.Helpers.Common
{
    public static class CommonHelper
    {
        #region EnumHelper
        public static class EnumHelper
        {
            public static string GetDescription(Enum en)
            {
                Type type = en.GetType();

                MemberInfo[] memInfo = type.GetMember(en.ToString());

                if (memInfo != null && memInfo.Length > 0)
                {
                    object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (attrs != null && attrs.Length > 0)
                    {
                        return ((DescriptionAttribute)attrs[0]).Description;
                    }
                }

                return en.ToString();
            }
            public static T GetEnum<T>(string description)
            {
                var type = typeof(T);
                if (!type.IsEnum) throw new InvalidOperationException();

                foreach (var field in type.GetFields())
                {
                    var attribute = Attribute.GetCustomAttribute(field,
                        typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attribute != null)
                    {
                        if (attribute.Description == description)
                            return (T)field.GetValue(null);
                    }
                    else
                    {
                        if (field.Name == description)
                            return (T)field.GetValue(null);
                    }
                }
                throw new ArgumentException("Not found.", "description");
                // or return default(T);
            }
            public static List<T> GetEnumValues<T>()
            {
                return Enum.GetValues(typeof(T)).Cast<T>().ToList();
            }
        }
        #endregion
        public static SelectList GetEnumSelectList<T>(object selectedValue = null)
        {
            Type t = typeof(T);
            if (t.IsEnum)
            {
                var items = from Enum e in Enum.GetValues(t)
                            select new { Value = Convert.ToInt32(e), Text = EnumHelper.GetDescription(e) };
                return (null == selectedValue ? new SelectList(items, "Value", "Text") : new SelectList(items, "Value", "Text", selectedValue));
            }
            return null;
        }
        public static string GetDisplayName<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression)
        {
            return ModelMetadata.FromLambdaExpression<TModel, TProperty>(
                expression,
                new ViewDataDictionary<TModel>(model)
                ).DisplayName;
        }

        #region Date and Time
        public class DateAndTimes
        {
            public static string GetPersianDateTime(object datetime)
            {
                string result = String.Empty;

                try
                {
                    PersianCalendar PerCal = new PersianCalendar();
                    DateTime dt = (DateTime)datetime;
                    result = PerCal.GetYear(dt).ToString();
                    result += "/" + (PerCal.GetMonth(dt) > 9 ? PerCal.GetMonth(dt).ToString() : String.Concat("0", PerCal.GetMonth(dt)));
                    result += "/" + (PerCal.GetDayOfMonth(dt) > 9 ? PerCal.GetDayOfMonth(dt).ToString() : String.Concat("0", PerCal.GetDayOfMonth(dt)));
                    result += "  " + (dt.TimeOfDay.Hours > 9 ? dt.TimeOfDay.Hours.ToString() : String.Concat("0", dt.TimeOfDay.Hours));
                    result += ":" + (dt.TimeOfDay.Minutes > 9 ? dt.TimeOfDay.Minutes.ToString() : String.Concat("0", dt.TimeOfDay.Minutes));

                    return result;
                }
                catch (Exception ex) { throw ex; }
            }
            public static string GetPersianDate(object datetime)
            {
                string result = String.Empty;

                try
                {
                    if (null == datetime) { return String.Empty; }

                    PersianCalendar PerCal = new PersianCalendar();
                    //DateTime dt = (DateTime)datetime;
                    DateTime dt = Convert.ToDateTime(datetime);
                    result = PerCal.GetYear(dt).ToString();
                    result += "/" + (PerCal.GetMonth(dt) > 9 ? PerCal.GetMonth(dt).ToString() : String.Concat("0", PerCal.GetMonth(dt)));
                    result += "/" + (PerCal.GetDayOfMonth(dt) > 9 ? PerCal.GetDayOfMonth(dt).ToString() : String.Concat("0", PerCal.GetDayOfMonth(dt)));

                    return result;
                }
                catch (Exception ex) { throw ex; }
            }
            public static DateTime GetDateTimePersianDate(object datetime)
            {
                string result = String.Empty;

                try
                {
                    PersianCalendar PerCal = new PersianCalendar();
                    //DateTime dt = (DateTime)datetime;
                    DateTime dt = Convert.ToDateTime(datetime);
                    return PerCal.ToDateTime(dt.Year, PerCal.GetMonth(dt), PerCal.GetDayOfMonth(dt), dt.TimeOfDay.Hours, dt.TimeOfDay.Minutes, dt.TimeOfDay.Seconds, 0);
                }
                catch (Exception ex) { throw ex; }
            }
            public static string GetYear(object datetime)
            {
                try
                {
                    if (null == datetime) { return String.Empty; }

                    PersianCalendar PerCal = new PersianCalendar();
                    //DateTime dt = (DateTime)datetime;
                    DateTime dt = Convert.ToDateTime(datetime);
                    return PerCal.GetYear(dt).ToString();
                }
                catch (Exception ex) { throw ex; }

            }
            public static string GetMonthName(DateTime date)
            {
                PersianCalendar jc = new PersianCalendar();
                string pdate = string.Format("{0:0000}/{1:00}/{2:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date));

                string[] dates = pdate.Split('/');
                int month = Convert.ToInt32(dates[1]);

                switch (month)
                {
                    case 1: return "فررودين";
                    case 2: return "اردیبهشت";
                    case 3: return "خرداد";
                    case 4: return "تیر‏";
                    case 5: return "مرداد";
                    case 6: return "شهریور";
                    case 7: return "مهر";
                    case 8: return "آبان";
                    case 9: return "آذر";
                    case 10: return "دی";
                    case 11: return "بهمن";
                    case 12: return "اسفند";
                    default: return "";
                }

            }
            public static string GetLastDayOfMonthPersian(string persiandate)
            {
                PersianCalendar pcal = new PersianCalendar();
                DateTime givenDate;
                DateTime lastDate;
                string[] aryDate = persiandate.Split('/');
                int year = Convert.ToInt32(aryDate[0]);
                int month = Convert.ToInt32(aryDate[1]);

                givenDate = pcal.ToDateTime(year, month, 1, 0, 0, 0, 0);
                lastDate = pcal.AddMonths(givenDate, 1);
                lastDate = pcal.AddDays(lastDate, -1);
                return GetPersianDate(lastDate);
            }
            public static DateTime GetStartDayOfMonth(DateTime tmpDatetime)
            {
                return new DateTime(tmpDatetime.Year, tmpDatetime.Month, 1);
            }
            public static DateTime GetLastDayOfMonth(DateTime tmpDatetime)
            {
                return new DateTime(tmpDatetime.Year, tmpDatetime.Month, 1).AddMonths(1).AddDays(-1);
            }
            public static DateTime? GetGregorianDate(string persianDate)
            {
                if (String.IsNullOrEmpty(persianDate)) { return null; }
                string[] aryDate = persianDate.Split(new char[] { '/', ' ', ':' });
                System.Globalization.PersianCalendar tmpPersianCalendar = new PersianCalendar();

                return aryDate.Length == 6 ? tmpPersianCalendar.ToDateTime(Convert.ToInt32(aryDate[0]), Convert.ToInt32(aryDate[1]), Convert.ToInt32(aryDate[2]), Convert.ToInt32(aryDate[4]), Convert.ToInt32(aryDate[5]), 0, 0)
                                           : tmpPersianCalendar.ToDateTime(Convert.ToInt32(aryDate[0]), Convert.ToInt32(aryDate[1]), Convert.ToInt32(aryDate[2]), 0, 0, 0, 0);
            }
            public static bool TryParse(string persianDate, out DateTime gregorianDate)
            {
                bool result = false;
                string[] aryDate = persianDate.Split('/');

                gregorianDate = DateTime.MinValue;

                if (!String.IsNullOrWhiteSpace(persianDate) &&
                    persianDate.Length == 10 &&
                    aryDate.Length == 3 &&
                    aryDate[0].Length == 4 &&
                    aryDate[1].Length == 2 &&
                    aryDate[2].Length == 2)
                {
                    result = true;
                    gregorianDate = (DateTime)CommonHelper.DateAndTimes.GetGregorianDate(persianDate);
                }

                return result;
            }
            public static string GetSortableDateTime(DateTime tmpDatetime)
            {
                string sortable = String.Empty;

                string year = tmpDatetime.Year.ToString();
                string month = tmpDatetime.Month > 9 ? tmpDatetime.Month.ToString() : String.Concat("0", tmpDatetime.Month);
                string day = tmpDatetime.Day > 9 ? tmpDatetime.Day.ToString() : String.Concat("0", tmpDatetime.Day);
                sortable = String.Concat(year, '-', month, '-', day, ' ', "00:00:00.000");

                return sortable;
            }
            public static string GetSortableDate(DateTime tmpDatetime)
            {
                string sortable = String.Empty;

                string year = tmpDatetime.Year.ToString();
                string month = tmpDatetime.Month > 9 ? tmpDatetime.Month.ToString() : String.Concat("0", tmpDatetime.Month);
                string day = tmpDatetime.Day > 9 ? tmpDatetime.Day.ToString() : String.Concat("0", tmpDatetime.Day);
                sortable = String.Concat(year, '-', month, '-', day);

                return sortable;
            }
        }
        #endregion

        public class ExposeProperty
        {
            public static PropertyInfo GetPropertyInfo<TSource, TProperty>(
                 TSource source,
                 Expression<Func<TSource, TProperty>> propertyLambda)
            {
                Type type = typeof(TSource);

                MemberExpression member = propertyLambda.Body as MemberExpression;
                if (member == null)
                    throw new ArgumentException(string.Format(
                        "Expression '{0}' refers to a method, not a property.",
                        propertyLambda.ToString()));

                PropertyInfo propInfo = member.Member as PropertyInfo;
                if (propInfo == null)
                    throw new ArgumentException(string.Format(
                        "Expression '{0}' refers to a field, not a property.",
                        propertyLambda.ToString()));

                if (type != propInfo.ReflectedType &&
                    !type.IsSubclassOf(propInfo.ReflectedType))
                    throw new ArgumentException(string.Format(
                        "Expresion '{0}' refers to a property that is not from type {1}.",
                        propertyLambda.ToString(),
                        type));

                return propInfo;
            }
        }
    }


}
