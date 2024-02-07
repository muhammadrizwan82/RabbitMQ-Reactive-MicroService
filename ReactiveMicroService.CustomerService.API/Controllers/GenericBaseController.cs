using Microsoft.AspNetCore.Mvc;
using ReactiveMicroService.CustomerService.API.DTO;
using ReactiveMicroService.CustomerService.API.Models;
using ReactiveMicroService.CustomerService.API.Service;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ReactiveMicroService.CustomerService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public abstract class GenericBaseController<T> : ControllerBase where T : BaseModel
    {
        private readonly GenericService<T> _genericService;
        private readonly TokenBlacklistService _blacklistService; 

        public GenericBaseController(GenericService<T> genericService, TokenBlacklistService blacklistService)
        {
            _genericService = genericService;
            _blacklistService = blacklistService;
        }
    
        protected IActionResult CreateResponse(int statusCode, bool success, string? message, object? data)
        {
            // When serializing or deserializing objects
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNameCaseInsensitive = true,
                // Other options as needed
            };

            var jsonString = JsonSerializer.Serialize(data, options);
            jsonString = Regex.Replace(jsonString, @"([\\])+", "");

            var response = new Response
            {
                StatusCode = statusCode,
                Success = success,
                Message = message,
                Data = jsonString
            };

            return StatusCode(statusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(T item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var controllerActionDescriptor = HttpContext.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();
                    var createdItem = await _genericService.CreateAsync(item, controllerActionDescriptor.ControllerName);
                    return CreateResponse(200, true, "Item created successfully", createdItem);
                }
                else
                {
                    return CreateResponse(400, false, "Item data is not valid", item);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(500, false, $"Error creating item: {ex.Message}", null);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var entity = await _genericService.GetAsync(id);
                if (entity == null)
                {
                    return CreateResponse(200, false, "Item not found", null);
                }

                return CreateResponse(200, true, "Item retrieved successfully", entity);
            }
            catch (Exception ex)
            {
                return CreateResponse(500, false, $"Error retrieving item: {ex.Message}", null);
            }
        }

        [HttpGet("GetByColumns")]
        public async Task<IActionResult> GetByColumns(Dictionary<string, object> filters)
        {
            try
            {
                var entity = await _genericService.GetByColumnFilter(filters);
                if (entity == null)
                {
                    return CreateResponse(200, false, "Item not found", null);
                }

                return CreateResponse(200, true, "Item retrieved successfully", entity);
            }
            catch (Exception ex)
            {
                return CreateResponse(500, false, $"Error retrieving item: {ex.Message}", null);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var entities = await _genericService.GetAllAsync();
                return CreateResponse(200, true, "Items retrieved successfully", entities);
            }
            catch (Exception ex)
            {
                return CreateResponse(500, false, $"Error retrieving items: {ex.Message}", null);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,T item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    var entity = await _genericService.GetAsync(id);
                    if (entity != null)
                    {
                        var controllerActionDescriptor = HttpContext.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();                        
                        var response = await _genericService.UpdateAsync(id, item, controllerActionDescriptor.ControllerName);
                        return CreateResponse(200, true, "Item updated successfully", response);
                    }
                    else
                    {
                        return CreateResponse(400, false, "Item not found", item);
                    }
                }
                else
                {
                    return CreateResponse(400, false, "Item data is not valid", item);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(500, false, $"Error updating item: {ex.Message}", null);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            try
            {
                var entity = await _genericService.GetAsync(id);
                if (entity != null)
                {
                    var controllerActionDescriptor = HttpContext.GetEndpoint().Metadata.GetMetadata<ControllerActionDescriptor>();
                    await _genericService.RemoveAsync(id, entity, controllerActionDescriptor.ControllerName);
                    _blacklistService.AddToBlacklist(HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());
                    return CreateResponse(200, true, "Item deleted successfully", null);
                }
                else
                {
                    _blacklistService.AddToBlacklist(HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());
                    return CreateResponse(400, false, "Item not found", null);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(500, false, $"Error deleting item: {ex.Message}", null);
            }
        }
    }
}
