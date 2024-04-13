using CsvHelper;
using IPLogsFilter.Converters;
using IPLogsFilter.Entity;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net;
namespace IPLogsFilter.Repository
{
    internal class IPLogsFileRepository : BaseRepository<IPLog>
    {
        public IPLogsFileRepository(IConfiguration config, IComparer<IPAddress> comparer) : base(config, comparer) { }
        public override async Task<IEnumerable<IPLog>> GetAllAsync()
        {
            using (var reader = new StreamReader(_fileLogPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records =   await csv.GetRecordsAsync<IPLog>().ToListAsync();
                return records;
            } 
        }

        public override IEnumerable<IPLog> GetByFilters(Filter filter)
        {
            var filetredIPs = new List<IPLog>();
                
                using (var reader = new StreamReader(_fileLogPath))
                using (var csv = new CsvReader(reader, _csvConf))
                {
                    csv.Context.RegisterClassMap<IPLogMap>();
                    while (csv.Read())
                    {
                        var record = csv.GetRecord<IPLog>();

                        if (record == null)
                            continue;
                        if (record.Date > filter.EndDate)
                            continue;
                        if (record.Date < filter.StartDate)
                            continue;

                        if (filter.StartIP != null && _comparer.Compare(filter.StartIP, record.IPAddress) > 0)
                                continue;
                        if (filter.EndIP != null && _comparer.Compare(filter.EndIP, record.IPAddress) < 0)
                            continue;

                        filetredIPs.Add(record);
                    }

                }
            
            return filetredIPs;
        }

      

        public override async Task CreateMultipleAsync(IEnumerable<LogCount> logs)
        {
            using (var sw = new StreamWriter(_fileOutputPath, append: false))
            using (var csv = new CsvWriter(sw, _csvConf))
            {
                await csv.WriteRecordsAsync(logs);

            }
        }



    }
}
