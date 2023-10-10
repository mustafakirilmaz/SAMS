using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SAMS.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        public static string[] StrSplit(string s, char separator)
        {
            if (s == null)
                return null;
            else
                return s.Split(separator);
        }

        public static bool TcDogrula(this string tcKimlikNo)
        {
            bool returnvalue = false;
            if (tcKimlikNo.Length == 11)
            {
                Int64 ATCNO, BTCNO, TcNo;
                long C1, C2, C3, C4, C5, C6, C7, C8, C9, Q1, Q2;

                TcNo = Int64.Parse(tcKimlikNo);

                ATCNO = TcNo / 100;
                BTCNO = TcNo / 100;

                C1 = ATCNO % 10; ATCNO = ATCNO / 10;
                C2 = ATCNO % 10; ATCNO = ATCNO / 10;
                C3 = ATCNO % 10; ATCNO = ATCNO / 10;
                C4 = ATCNO % 10; ATCNO = ATCNO / 10;
                C5 = ATCNO % 10; ATCNO = ATCNO / 10;
                C6 = ATCNO % 10; ATCNO = ATCNO / 10;
                C7 = ATCNO % 10; ATCNO = ATCNO / 10;
                C8 = ATCNO % 10; ATCNO = ATCNO / 10;
                C9 = ATCNO % 10; ATCNO = ATCNO / 10;
                Q1 = ((10 - ((((C1 + C3 + C5 + C7 + C9) * 3) + (C2 + C4 + C6 + C8)) % 10)) % 10);
                Q2 = ((10 - (((((C2 + C4 + C6 + C8) + Q1) * 3) + (C1 + C3 + C5 + C7 + C9)) % 10)) % 10);

                returnvalue = ((BTCNO * 100) + (Q1 * 10) + Q2 == TcNo);
            }
            return returnvalue;
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static bool IsValidTC(this string tcKimlikNo)
        {
            bool returnvalue = false;
            if (tcKimlikNo.Length == 11)
            {
                Int64 ATCNO, BTCNO, TcNo;
                long C1, C2, C3, C4, C5, C6, C7, C8, C9, Q1, Q2;
                try
                {
                    TcNo = Int64.Parse(tcKimlikNo);
                }
                catch (Exception ex)
                {
                    return returnvalue;
                }


                ATCNO = TcNo / 100;
                BTCNO = TcNo / 100;

                C1 = ATCNO % 10; ATCNO = ATCNO / 10;
                C2 = ATCNO % 10; ATCNO = ATCNO / 10;
                C3 = ATCNO % 10; ATCNO = ATCNO / 10;
                C4 = ATCNO % 10; ATCNO = ATCNO / 10;
                C5 = ATCNO % 10; ATCNO = ATCNO / 10;
                C6 = ATCNO % 10; ATCNO = ATCNO / 10;
                C7 = ATCNO % 10; ATCNO = ATCNO / 10;
                C8 = ATCNO % 10; ATCNO = ATCNO / 10;
                C9 = ATCNO % 10; ATCNO = ATCNO / 10;
                Q1 = ((10 - ((((C1 + C3 + C5 + C7 + C9) * 3) + (C2 + C4 + C6 + C8)) % 10)) % 10);
                Q2 = ((10 - (((((C2 + C4 + C6 + C8) + Q1) * 3) + (C1 + C3 + C5 + C7 + C9)) % 10)) % 10);

                returnvalue = ((BTCNO * 100) + (Q1 * 10) + Q2 == TcNo);
            }
            return returnvalue;
        }

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

        public static string[] SplitNameSurname(this string parsingNameSurname)
        {
            string[] nameSurname = new string[2];
            nameSurname[0] = "";
            nameSurname[1] = "";

            string[] parsedNameSurname = parsingNameSurname.Split(' ');

            if (parsedNameSurname.Length == 1)
            {
                nameSurname[0] = parsedNameSurname[0];
            }
            else if (parsedNameSurname.Length == 2)
            {
                nameSurname[0] = parsedNameSurname[0];
                nameSurname[1] = parsedNameSurname[1];
            }
            else
            {
                nameSurname[0] = parsedNameSurname[0];
                nameSurname[1] = string.Join(" ", parsedNameSurname.Skip(1));
            }

            return nameSurname;
        }

        public static string ToPhoneNumber(this String text)
        {
            string returnTel = null;
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            else
            {
                returnTel = text.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace(".", "").Replace("/", "").Replace(@"\", "").TrimStart().TrimEnd();
                if (text.StartsWith("0"))
                {
                    returnTel = text.Substring(1);
                }
            }
            return returnTel;
        }

        public static string IfEmptyGetNull(this String text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }
            else
                return text;
        }

        public static string MaskTextByAsterisk(string text, int showCharCount = 2)
        {
            if (text != null)
            {
                var length = text.Length;
                StringBuilder builder = new StringBuilder();

                if (length > showCharCount * 2)
                {
                    builder.Append(text.Substring(0, showCharCount));
                    for (int i = 0; i < length - (showCharCount * 2); i++)
                    {
                        builder.Append("*");
                    }
                    builder.Append(text.Substring(length - (showCharCount), showCharCount));
                    return builder.ToString();
                }
                else
                {
                    return text;
                }
            }
            else
            {
                return "";
            }
           

        }
    }

}
