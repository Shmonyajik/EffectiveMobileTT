using CsvHelper.Configuration.Attributes;
using IPLogsFilter.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPLogsFilter.Entity
{
    [HasHeaderRecord(true)]
    public class LogCount
    {
        [Name("IPAddress")]
        [TypeConverter(typeof(IPAddressConverter))]
        public IPAddress IPAddress {get;set;}

        [Name("Count")]
        public int Count { get;set;}
    }
}
