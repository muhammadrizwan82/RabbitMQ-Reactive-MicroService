using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Plain.RabbitMQ;
using ReactiveMicroService.OrderService.API.DTO;
using ReactiveMicroService.OrderService.API.Models;
using ReactiveMicroService.OrderService.API.Service;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReactiveMicroService.OrderService.API.Controllers
{
    public class OrderController : GenericBaseController<Orders>
    {
        private readonly OrdersService _orderService; 

        public OrderController(OrdersService orderService) : base(orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("CreateNewOrder")]
        public async Task<IActionResult> CreateNewOrder(OrderDTO orderDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var createdItem = await _orderService.CreateNewOrder(orderDTO);
                    var options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        PropertyNameCaseInsensitive = true,
                        // Other options as needed
                    };
                                        
                    return CreateResponse(201, true, "Item created successfully", createdItem);
                }
                else
                {
                    return CreateResponse(400, false, "Item data is not valid", orderDTO);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(500, false, $"Error creating order: {ex.ToString()}", null);
            }
        }

        [HttpGet("GetOrderSummary/{OrderId}")]
        public async Task<IActionResult> GetOrderSummary(int OrderId)
        {
            try
            {
                var entity = await _orderService.GetOrderSummary(OrderId);
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
    }
}
