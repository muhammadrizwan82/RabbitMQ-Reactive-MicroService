
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plain.RabbitMQ;
using ReactiveMicroService.CustomerService.API.DTO;
using ReactiveMicroService.CustomerService.API.Models;
using ReactiveMicroService.CustomerService.API.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ReactiveMicroService.CustomerService.API.Controllers
{  
    public class CustomerAddressController : GenericBaseController<CustomerAddresses>
    {
        private readonly CustomerAddressesService _customerAddressesService;
        private readonly IPublisher _publisher;
        private readonly UtilityService _utilityService;
        private readonly TokenBlacklistService _blacklistService;

        public CustomerAddressController(CustomerAddressesService customerAddressesService, IPublisher publisher, UtilityService utilityService
            , TokenBlacklistService blacklistService) : base(customerAddressesService, blacklistService)
        {
            _customerAddressesService = customerAddressesService;
            _publisher = publisher;
            _utilityService = utilityService;
            _blacklistService = blacklistService;
        }

        [HttpPost("CreateCustomerAddress")]
        public async Task<IActionResult> CreateCustomerAddress(CustomerAddressDTO customerAddressDTO )
        {
            try
            {
                if (ModelState.IsValid)
                {                    
                    var createdItem = await _customerAddressesService.AddCustomerAddress(customerAddressDTO, 
                        int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                    if (createdItem.Id == 0)
                    {
                        return CreateResponse(400, true, "Item not created", null);
                    }
                    else
                    {
                        return CreateResponse(200, true, "Item created successfully", createdItem);
                    }
                }
                else
                {
                    return CreateResponse(400, false, "Item data is not valid", null);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(500, false, $"Error creating item: {ex.Message}", null);
            }
        }        
    }
}
