using CsvHelper.Configuration.Attributes;
using IPLogsFilter.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IPLogsFilter.Entity
{
    [HasHeaderRecord(true)]
    public class IPLog
    {
        [Name("Date")]
        [TypeConverter(typeof(DateTimeConverter))]
        public DateTime Date {  get; set; }
        
        [Name("IPAddress")]
        [TypeConverter(typeof(IPAddressConverter))]
        public IPAddress IPAddress { get; set; }
       
    }

    
}
