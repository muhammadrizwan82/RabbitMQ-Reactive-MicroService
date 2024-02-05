using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Management;
using System.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ReactiveMicroService.ReportService.API.Models;
using Plain.RabbitMQ;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReactiveMicroService.ReportService.API.Service
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
        public string GetUserAgent()
        {
            string userAgent = "";
            userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"];
            return userAgent;
        }
         
         
    }
}
