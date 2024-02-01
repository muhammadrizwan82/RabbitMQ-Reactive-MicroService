using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Management;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ReactiveMicroService.CustomerService.API.Models;
using Plain.RabbitMQ;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReactiveMicroService.CustomerService.API.Service
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
            try { 
                var token = await httpContext.GetTokenAsync("access_token");
                if (token != null)
                {
                    return token.ToString();
                }
                else {
                    return "";
                }
            }
            catch (Exception ex) {
                return ex.Message;
            }
            
        }

        public string GenerateJwtToken(Customers customers)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iConfiguration["Jwt:SecretKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: _iConfiguration["Jwt:Issuer"], 
                audience: _iConfiguration["Jwt:Audience"], 
                claims: new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, customers.Id.ToString()),
                    new Claim(ClaimTypes.Email, customers.EmailAddress)
                }, expires: DateTime.UtcNow.AddHours(24), signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return tokenString; 
        }

        public async Task<int> GetAuthorizeCustomerId(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            // Parse and validate JWT token to get user information
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            return int.Parse(jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        }

        public async Task AddDatatoQueue(object item,string rotuingKey) {
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
