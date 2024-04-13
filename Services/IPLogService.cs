using IPLogsFilter.Converters;
using IPLogsFilter.Entity;
using IPLogsFilter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPLogsFilter.Services
{
    public class IPLogService : ILogService<IPLog>
    {
        private readonly BaseRepository<IPLog> _repository;
        public IPLogService(BaseRepository<IPLog> repository)
        {
            _repository = repository;
        }

        public IEnumerable<LogCount> GetByFilters(string? _startDate, string? _endDate, string? _startIP, string? _mask)
        {
            var filter = GetFilterFromStringArgs(_startDate, _endDate, _startIP, _mask);
            var logs =  _repository.GetByFilters(filter);
  
            var numberLogsByIP = logs.GroupBy(x=>x.IPAddress)
                .Select(group => new LogCount
                {
                    IPAddress = group.Key,
                    Count = group.Count()
                }).ToList();

            return numberLogsByIP;
        }

        public async Task CreateAsync(IEnumerable<LogCount> logs)
        {
            await _repository.CreateMultipleAsync(logs);
        }

        //Валидация по бизнес процессам
        private Filter GetFilterFromStringArgs(string? _startDate, string? _endDate, string? _startIP, string? _mask)
        {
            var filter = new Filter();

            if (_startDate == null)
                throw new ArgumentNullException("Diapazon start date not found in configuration");
            else
                filter.StartDate = (DateTime)new DateTimeConverter().ConvertFromString(_startDate);
            if (_endDate == null)
                throw new ArgumentNullException("Diapazon end date not found in configuration");
            else
                filter.EndDate = (DateTime)new DateTimeConverter().ConvertFromString(_endDate);
            if (filter.StartDate > filter.EndDate)
                throw new ArgumentException($"The start date of the range cannot be greater than the end date: {filter.StartDate}-{filter.EndDate}");
            if (!string.IsNullOrWhiteSpace(_startIP))
            {
                filter.StartIP = (IPAddress)new IPAddressConverter().ConvertFromString(_startIP);

            }
            if (!string.IsNullOrWhiteSpace(_mask))
            {
                if (string.IsNullOrWhiteSpace(_startIP))
                {
                    throw new ArgumentException("Сannot apply a mask when the starting IP address of the range is not specified");
                }
                if (!int.TryParse(_mask, out int mask))
                {
                    throw new FormatException($"Could not convert '{_mask}' to type int.");
                }
                if (mask < 0 || mask > 32)
                {
                    throw new ArgumentException("Incorrect Subnet Mask number.");
                }
            }
            filter.StartIP = IPAddress.Parse(_startIP!);
            filter.EndIP = CalculateEndIpAddress(filter.StartIP, int.Parse(_mask!));

            return filter;
        }

        private IPAddress CalculateEndIpAddress(IPAddress ip, int subnetMask)
        {
            uint ipValue = BitConverter.ToUInt32(ip.GetAddressBytes().Reverse().ToArray(), 0);
            uint mask = 0xFFFFFFFF << (32 - subnetMask);
            #region Проверка на соответствие
            //if ((ipValue & ~mask) == 0)
            //{
            //    throw new ArgumentException("Неверный номер маски подсети.");
            //}
            #endregion
            uint endIpValue = ipValue | ~mask;

            return new IPAddress(BitConverter.GetBytes(endIpValue).Reverse().ToArray());
        }
    }
}
