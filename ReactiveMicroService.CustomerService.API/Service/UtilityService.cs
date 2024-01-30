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

namespace ReactiveMicroService.CustomerService.API.Service
{
    public class UtilityService 
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _iConfiguration;
        public UtilityService(IHttpContextAccessor httpContextAccessor, IConfiguration iConfiguration)
        {
            _httpContextAccessor = httpContextAccessor;
            _iConfiguration = iConfiguration;
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
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_iConfiguration["Jwt:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, customers.Id.ToString()),
                new Claim(ClaimTypes.Name, customers.EmailAddress)
                }),
                Expires = DateTime.UtcNow.AddHours(48),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
