using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Plain.RabbitMQ;
using Microsoft.Extensions.Configuration;

namespace ReactiveMicroService.OrderService.API.Service
{
    public class UtilityService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _iConfiguration;
        private readonly IPublisher _publisher;

        public UtilityService(IHttpContextAccessor httpContextAccessor, IConfiguration iConfiguration, IPublisher publisher)
        {
            _httpContextAccessor = httpContextAccessor;
            _iConfiguration = iConfiguration;
            _publisher = publisher;
        }
        public string? GetClientIP()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            return Dns.GetHostByName(hostName).AddressList[0].ToString();
        }

        public async Task AddDatatoQueue(object item, string rotuingKey)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNameCaseInsensitive = true,
                // Other options as needed
            };
            _publisher.Publish(JsonSerializer.Serialize(item, options), rotuingKey, null);
        }
    }
}
