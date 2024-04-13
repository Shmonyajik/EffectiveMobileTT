using CsvHelper.Configuration;
using IPLogsFilter.Entity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPLogsFilter.Repository
{
    public abstract class BaseRepository <T> where T : class
    {
        protected readonly string _fileLogPath;
        protected readonly string _fileOutputPath;
        protected readonly CsvConfiguration _csvConf;
        protected readonly IComparer<IPAddress> _comparer;

        public BaseRepository(IConfiguration config, IComparer<IPAddress> comparer)
        {
            var fileLogPath = config.GetSection("configuration:file-log").Value;
            var fileOutputPath = config.GetSection("configuration:file-output").Value;
            if (string.IsNullOrEmpty(fileLogPath))
                _fileLogPath = Environment.GetCommandLineArgs()[2];
            else
                _fileLogPath = fileLogPath;

            if (string.IsNullOrEmpty(fileOutputPath))
                _fileOutputPath = Environment.GetCommandLineArgs()[4];
            else
                _fileOutputPath = fileOutputPath;

            _csvConf = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                HasHeaderRecord = true
            };
            _comparer = comparer;
            
        }

        public abstract Task<IEnumerable<T>> GetAllAsync();

        public abstract IEnumerable<T> GetByFilters(Filter filter);

        public abstract Task CreateMultipleAsync(IEnumerable<LogCount> logs); 
    }
}
