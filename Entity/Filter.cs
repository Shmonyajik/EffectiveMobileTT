
using System.Net;


namespace IPLogsFilter.Entity
{
    public class Filter
    {
        public IPAddress? StartIP { get; set; }
        public IPAddress? EndIP { get; set; }
        public DateTime StartDate { get; set; }
        //TODO: Перенести валидацию в метод
        public DateTime EndDate { get; set; }

    }
}
