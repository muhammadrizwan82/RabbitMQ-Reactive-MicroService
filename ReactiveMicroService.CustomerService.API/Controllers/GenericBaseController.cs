using Microsoft.AspNetCore.Mvc;
using ReactiveMicroService.CustomerService.API.DTO;
using ReactiveMicroService.CustomerService.API.Models;
using ReactiveMicroService.CustomerService.API.Service;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ReactiveMicroService.CustomerService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class GenericBaseController<T> : ControllerBase where T : BaseModel
    {
        private readonly GenericService<T> _genericService;

        public GenericBaseController(GenericService<T> genericService)
        {
            _genericService = genericService;
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
                    var createdItem = await _genericService.CreateAsync(item);                    
                    return CreateResponse(201, true, "Item created successfully", createdItem);
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

        [HttpGet("GetOrderByKeyValue")]
        public async Task<IActionResult> GetByColumns(Dictionary<string,object> filters)
        {
            try
            {           
                var entity = await _genericService.FilterAsync(filters);
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

        [HttpGet]
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
        public async Task<IActionResult> Update(int id, T item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await _genericService.GetAsync(id);
                    if (entity != null)
                    {                        
                        var response = await _genericService.UpdateAsync(id,item);                        
                        return CreateResponse(200, true, "Item updated successfully", response);
                    }
                    else {
                        return CreateResponse(400, false, "Item data is not valid", item);
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
                    await _genericService.RemoveAsync(id, entity);                    
                    return CreateResponse(200, true, "Item deleted successfully",null);
                }
                else
                {
                    return CreateResponse(400, false, "Item data is not valid", null);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(500, false, $"Error deleting item: {ex.Message}", null);
            }
        }
    }
}
