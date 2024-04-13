using IPLogsFilter.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPLogsFilter.Services
{
    public interface ILogService<T>
    {
        IEnumerable<LogCount> GetByFilters(string? _startDate, string? _endDate, string? _startIP, string? _mask);

        Task CreateAsync(IEnumerable<LogCount> logs);
    }
}
