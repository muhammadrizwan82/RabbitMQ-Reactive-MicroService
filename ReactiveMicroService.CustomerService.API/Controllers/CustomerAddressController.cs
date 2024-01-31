
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

        public CustomerAddressController(CustomerAddressesService customerAddressesService, IPublisher publisher) : base(customerAddressesService)
        {
            _customerAddressesService = customerAddressesService;
            _publisher = publisher;
        }

        [HttpPost("CreateCustomerAddress")]
        public async Task<IActionResult> CreateCustomerAddress(CustomerAddressDTO customerAddressDTO )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                    // Parse and validate JWT token to get user information
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                    var customerId = int.Parse(jsonToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                    var createdItem = await _customerAddressesService.AddCustomerAddress(customerAddressDTO, customerId);
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
