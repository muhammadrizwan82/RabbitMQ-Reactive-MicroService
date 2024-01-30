using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Plain.RabbitMQ;
using ReactiveMicroService.CustomerService.API.DTO;
using ReactiveMicroService.CustomerService.API.Models;
using ReactiveMicroService.CustomerService.API.Service;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReactiveMicroService.CustomerService.API.Controllers
{
    public class CustomerController : GenericBaseController<Customers>
    {
        private readonly CustomersService _customersService;
        private readonly IPublisher _publisher;

        public CustomerController(CustomersService customersService, IPublisher publisher) : base(customersService)
        {
            _customersService = customersService;
            _publisher = publisher;
        }


        [HttpPost("Signup")]
        public async Task<IActionResult> Signup(CustomerSignupDTO customerDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var createdItem = await _customersService.Signup(customerDTO, HttpContext);
                    if (createdItem.Id == 0) {
                        return CreateResponse(400, true, "Email already exists", customerDTO);
                    }
                    else
                    {
                        return CreateResponse(200, true, "Item created successfully", createdItem);
                    }
                }
                else
                {
                    return CreateResponse(400, false, "Item data is not valid", customerDTO);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(500, false, $"Error creating item: {ex.Message}", null);
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var createdItem = await _customersService.Login(loginDTO, HttpContext);
                    if (createdItem.EmailAddress != loginDTO.EmailAddress)
                    {
                        return CreateResponse(400, true, "Item data is not valid", loginDTO);
                    }
                    else
                    {
                        return CreateResponse(200, true, "login successfully", createdItem);
                    }
                }
                else
                {
                    return CreateResponse(400, false, "Item data is not valid", loginDTO);
                }
            }
            catch (Exception ex)
            {
                return CreateResponse(500, false, $"Error creating item: {ex.Message}", null);
            }
        }
    }
}
