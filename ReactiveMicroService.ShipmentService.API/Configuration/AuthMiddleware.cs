﻿using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.IdentityModel.Tokens;
using ReactiveMicroService.ShipmentService.API.DTO;
using ReactiveMicroService.ShipmentService.API.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReactiveMicroService.ShipmentService.API.Configuration
{
    public class AuthMiddleware : IMiddleware
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IConfiguration _configuration;
        private readonly TokenBlacklistService _blacklistService;

        public AuthMiddleware(IConfiguration configuration, TokenBlacklistService blacklistService)
        {
            _configuration = configuration;
            _tokenHandler = new JwtSecurityTokenHandler();
            _tokenValidationParameters = new TokenValidationParameters();
            _blacklistService = blacklistService;

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
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNameCaseInsensitive = true,
                // Other options as needed
            };
            await next(context);

            /*
                         var controllerActionDescriptor = context.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();

            if (controllerActionDescriptor == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                var response = JsonSerializer.Serialize(new LoginToken() { Message = "Method not allwoed" }, options);
                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                await context.Response.WriteAsync(response);
                return;
            }
            if (controllerActionDescriptor.ControllerName != "customers" && (controllerActionDescriptor.ActionName.ToLower() != "login" &&
                    controllerActionDescriptor.ActionName.ToLower() != "signup"))
            {

                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    var response = JsonSerializer.Serialize(new LoginToken() { Message = "Invalid token" }, options);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(response);
                    return;
                }

                if (_blacklistService.IsTokenBlacklisted(token))
                {
                    // Token is blacklisted, reject the request
                    var response = JsonSerializer.Serialize(new LoginToken() { Message = "Invalid token" }, options);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync(response);
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
                await next(context);
            }
             */
        }
    }
}
