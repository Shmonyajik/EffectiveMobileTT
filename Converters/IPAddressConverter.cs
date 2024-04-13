using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPLogsFilter.Converters
{
    internal class IPAddressConverter : ITypeConverter 
    {
        public object ConvertFromString(string text, IReaderRow? row = default, MemberMapData? memberMapData = default)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            if (IPAddress.TryParse(text, out IPAddress ipAddress))
                return ipAddress;

            throw new FormatException($"Could not convert '{text}' to type IPAddress.");
        }

        public string ConvertToString(object value, IWriterRow? row = default, MemberMapData? memberMapData = default)
        {
            return value?.ToString();
        }
    }
}
