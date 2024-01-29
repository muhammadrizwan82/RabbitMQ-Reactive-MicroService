using System.Net;

namespace ReactiveMicroService.CustomerService.API.Service
{
    public class UtilityService
    {
        public string? GetClientIP()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            return Dns.GetHostByName(hostName).AddressList[0].ToString();
        }
    }
}
