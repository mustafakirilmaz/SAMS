using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace SAMS.Common.Helpers
{
    public static class IpAddressHelper
    {

        public static string GetLocalIpAddress()
        {
            var addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(addr => addr.AddressFamily == AddressFamily.InterNetwork).Select(addr => addr.ToString()).ToList();

            foreach (var ipAddress in addresses.Where(ipAddress => !ipAddress.StartsWith("169")))
            {
                return ipAddress;
            }
            return addresses.FirstOrDefault();
        }

        public static List<string> GetAllLocalIpAddresses()
        {
            var addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(addr => addr.AddressFamily == AddressFamily.InterNetwork).Select(addr => addr.ToString()).ToList();

            return addresses;
        }

    }

}
