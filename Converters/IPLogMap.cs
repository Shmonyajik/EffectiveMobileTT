using CsvHelper.Configuration;
using IPLogsFilter.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPLogsFilter.Converters
{
    public sealed class IPLogMap : ClassMap<IPLog>
    {
        public IPLogMap()
        {
            Map(m => m.Date).Name("Date").TypeConverter<DateTimeConverter>();
            Map(m => m.IPAddress).Name("IPAddress").TypeConverter<IPAddressConverter>();
        }
    }
}
