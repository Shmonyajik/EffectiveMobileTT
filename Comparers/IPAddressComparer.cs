using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IPLogsFilter.Comparers
{
    public class IPAddressComparer : IComparer<IPAddress>
    {
        public int Compare(IPAddress x, IPAddress y)
        {
            byte[] xBytes = x.GetAddressBytes();
            byte[] yBytes = y.GetAddressBytes();

            // Сравнение по порядку байтов
            for (int i = 0; i < xBytes.Length; i++)
            {
                if (xBytes[i] < yBytes[i])
                    return -1;
                else if (xBytes[i] > yBytes[i])
                    return 1;
            }

            return 0; // Если адреса равны
        }
    }
}
