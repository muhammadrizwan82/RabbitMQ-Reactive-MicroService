using Microsoft.AspNetCore.Mvc;
using ReactiveMicroService.ShipmentService.API.DTO;
using ReactiveMicroService.ShipmentService.API.Models;
using ReactiveMicroService.ShipmentService.API.Service;

namespace ReactiveMicroService.ShipmentService.API.Controllers
{    
    public class ShipmentController : GenericBaseController<Shipments>
    {
        private readonly ShipmentsService _shipmentService;
        private readonly TokenBlacklistService _tokenBlacklistService;

        public ShipmentController(ShipmentsService shipmentService,TokenBlacklistService tokenBlacklistService) : base(shipmentService, tokenBlacklistService)
        {
            _tokenBlacklistService = tokenBlacklistService;
            _shipmentService = shipmentService;
        }
        
        [HttpPost("CreateShipment")]
        public async Task<IActionResult> CreateShipment(ShipmentDTO shipmentDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var createdItem = await _shipmentService.CreateShipmentAsync(shipmentDTO, HttpContext);
                    if (createdItem.Id == 0)
                    {
                        return CreateResponse(400, true, "Order shipment already exists", shipmentDTO);
                    }
                    else
                    {
                        return CreateResponse(200, true, "Item created successfully", createdItem);
                    }
                }
                else
                {
                    return CreateResponse(400, false, "Item data is not valid", shipmentDTO);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(500, false, $"Error creating item: {ex.Message}", null);
            }
        }
    }
}