
using System;

namespace SAMS.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime? SetAsDayEndTime(this DateTime? date)
        {
            if (date == null)
            {
                return null;
            }

            return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 23, 59, 59);
        }

        public static DateTime? SetAsDayStartTime(this DateTime? date)
        {
            if (date == null)
            {
                return null;
            }

            return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, 0, 0, 0);
        }

        public static DateTime SetAsDayEndTime(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        public static DateTime SetAsDayStartTime(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static DateTime? ToDateTime(this DateTime? date)
        {
            if (date == null)
            {
                return null;
            }
            else
            {
                if (date.Value.Year < 1800)
                {
                    return null;
                }
                else
                {
                    return date;
                }
            }
        }

        public static int? CalculateAge(this DateTime? birthDate)
        {
            if (birthDate == null)
            {
                return null;
            }
            else
            {
                int age = DateTime.Now.Year - birthDate.Value.Year;
                if (birthDate > DateTime.Now.AddYears(-age))
                {
                    age--;
                }
                return age;

            }
        }

        public static DateTime ConvertStringToDate(this string dateString)
        {
            string[] parsedDate = dateString.Split('/');

            int year = Convert.ToInt32(parsedDate[2]);
            int month = Convert.ToInt32(parsedDate[1]);
            int day = Convert.ToInt32(parsedDate[0]);

            return new DateTime(year, month, day);
        }

        public static DateTime ConvertStringToDateTime(this string dateString)
        {
            string[] parsedValues = dateString.Split('T');

            string[] parsedDate = parsedValues[0].Split('/');
            if (parsedDate.Length == 1)
            {
                parsedDate = parsedValues[0].Split('.');
            }
            int year = Convert.ToInt32(parsedDate[2]);
            int month = Convert.ToInt32(parsedDate[1]);
            int day = Convert.ToInt32(parsedDate[0]);

            if (parsedValues.Length > 1)
            {
                string[] parsedTime = parsedValues[1].Split(':');

                int hour = Convert.ToInt32(parsedTime[0]);
                int minute = Convert.ToInt32(parsedTime[1]);
                int second = 0;
                if (parsedTime.Length == 3)
                {
                    second = Convert.ToInt32(parsedTime[2]);
                }

                return new DateTime(year, month, day, hour, minute, second);
            }

            return new DateTime(year, month, day);
        }

        public static string GetOnlyDateAsString(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        public static string GetOnlyTimeAsString(this DateTime date)
        {
            return date.ToString("HH:mm");
        }

        public static double GetTotalSeconds(this DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static DateTime GetDateTime(this double timeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(timeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static int MonthDifference(this DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }
    }
}
