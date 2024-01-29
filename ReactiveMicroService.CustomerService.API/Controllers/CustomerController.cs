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

         
    }
}
