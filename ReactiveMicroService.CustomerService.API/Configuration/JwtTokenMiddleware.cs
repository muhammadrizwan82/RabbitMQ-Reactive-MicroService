using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReactiveMicroService.CustomerService.API.Configuration
{
    public class JwtTokenMiddleware : IMiddleware
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IConfiguration _configuration;

        public JwtTokenMiddleware(IConfiguration configuration)
        {
            _configuration = configuration;
            _tokenHandler = new JwtSecurityTokenHandler();

            // Configure token validation parameters
            _tokenValidationParameters = new TokenValidationParameters
            {

                ValidateIssuerSigningKey = true,                
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidIssuer = configuration["Jwt:Issuer"], // Replace with your issuer
                ValidAudience = configuration["Jwt:Audience"], // Replace with your audience
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])) // Replace with your secret key
            };

        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var controllerActionDescriptor = context.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();
             
            if (controllerActionDescriptor.ControllerName != "customers" && controllerActionDescriptor.ActionName.ToLower() != "login")
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Missing JWT token");
                    return;
                }

                try
                {
                    // Validate token
                    SecurityToken validatedToken;
                    var principal = _tokenHandler.ValidateToken(token, _tokenValidationParameters, out validatedToken);

                    // Extract claims
                    var customerId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var emailAddress = principal.FindFirst(ClaimTypes.Email)?.Value;
                    // Extract other claims as needed

                    // Add claims to the HttpContext for further processing
                    context.Items["UserId"] = customerId;
                    context.Items["Username"] = emailAddress;

                    await next(context);
                }
                catch (SecurityTokenException)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid JWT token");
                    return;
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync($"Internal server error: {ex.Message}");
                    return;
                }
            }
            else
            {
                await next(context); ;
            }
        }
    }
}
