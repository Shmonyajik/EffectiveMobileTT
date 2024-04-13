using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.TypeConversion;

namespace IPLogsFilter.Converters
{
    internal class DateTimeConverter : ITypeConverter
    {
        //Всегда возвращает DateTime в формате "yyyy-MM-dd HH:mm:ss или null"
        public object ConvertFromString(string text, IReaderRow? row = default, MemberMapData? memberMapData = null)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;
            
            if (DateTime.TryParseExact(text, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
            {
                
                return DateTime.ParseExact(dateTime.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null);
            }
         
            if (DateTime.TryParseExact(text, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out dateTime))
                return dateTime;

            throw new FormatException($"Could not convert '{text}' to type DateTime.");
        }

        public string ConvertToString(object value, IWriterRow? row = default, MemberMapData? memberMapData = default)
        {
            if (value == null)
                return string.Empty;

            return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
