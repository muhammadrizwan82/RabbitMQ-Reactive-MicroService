using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Plain.RabbitMQ;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace ReactiveMicroService.ShipmentService.API.Service
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
            return Dns.GetHostEntry(hostName).AddressList[0].ToString();
        }
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
        public string GetUserAgent()
        {
            string userAgent = "";
            userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"];
            return userAgent;
        }

        public async Task<string> GetDeviceTokenAsync(HttpContext httpContext)
        {
            try
            {
                var token = await httpContext.GetTokenAsync("access_token");
                if (token != null)
                {
                    return token.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

    

        public async Task<int> GetAuthorizeCustomerId(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // Parse and validate JWT token to get user information
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            return int.Parse(jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        }

        public async Task AddDatatoQueue(object item, string rotuingKey, Dictionary<string, object> headers)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNameCaseInsensitive = true,
                // Other options as needed
            };

            _publisher.Publish(JsonSerializer.Serialize(item, options), rotuingKey, headers, "30000");
        }
    }
}
