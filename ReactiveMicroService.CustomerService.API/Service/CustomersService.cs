
using ReactiveMicroService.CustomerService.API.DTO;
using ReactiveMicroService.CustomerService.API.Models;
using ReactiveMicroService.CustomerService.API.Repository;
using System.Text.Json.Serialization;
using System.Text.Json;
using Plain.RabbitMQ;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace ReactiveMicroService.CustomerService.API.Service
{
    public class CustomersService : GenericService<Customers>
    {
        private readonly IGenericRepository<Customers> _customerRepository;
        private readonly IGenericRepository<CustomerDevices> _customerDeviceRepository;
        private readonly UtilityService _utilityService;

        public CustomersService(
            IGenericRepository<Customers> customerRepository,
            IGenericRepository<CustomerDevices> customerDeviceRepository,
            UtilityService utilityService) : base(customerRepository, utilityService)
        {
            _customerRepository = customerRepository;
            _customerDeviceRepository = customerDeviceRepository;
            _utilityService = utilityService;
        }

        public async Task<Customers> Signup(CustomerSignupDTO customerDTO, HttpContext httpContext)
        {
            var insertedCustomer = new Customers();
            if (await _customerRepository.GetByColumnsFirstOrDefault(new Dictionary<string, object>
            {
                { "EmailAddress", customerDTO.EmailAddress }
            }) == null)
            {
                insertedCustomer = await _customerRepository.Insert(new Customers()
                {
                    Password = _utilityService.HashPassword(customerDTO.Password),
                    CreatedIP = _utilityService.GetClientIP(),
                    DialCode = customerDTO.DialCode,
                    EmailAddress = customerDTO.EmailAddress,
                    FirstName = customerDTO.FirstName,
                    LastName = customerDTO.LastName,
                    FullName = $"{customerDTO.FirstName} {customerDTO.LastName}",
                    PhoneNumber = customerDTO.PhoneNumber.ToString(),
                    CreatedBy = 1,
                    CreatedAt = DateTime.UtcNow
                });
                if (insertedCustomer != null)
                {
                    await _utilityService.AddDatatoQueue(insertedCustomer, "report.customer"
                        , new Dictionary<string, object> { { "customer", "new" } });

                    var existingDevice = await _customerDeviceRepository.GetByColumnsFirstOrDefault(new Dictionary<string, object>
                    {
                        { "DeviceId", customerDTO.DeviceId },
                        { "CustomerId", insertedCustomer.Id },
                        { "IsActive", true },
                        { "IsDeleted", false },
                    });
                    if (existingDevice == null)
                    {
                        var insertedCustomerDevice = new CustomerDevices();
                        var customerDeviceList = new List<CustomerDevices>();
                        insertedCustomerDevice = await _customerDeviceRepository.Insert(new CustomerDevices()
                        {
                            CreatedIP = _utilityService.GetClientIP(),
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = insertedCustomer.Id,
                            CustomerId = insertedCustomer.Id,
                            DeviceId = customerDTO.DeviceId,
                            UserAgent = _utilityService.GetUserAgent(),
                            DeviceToken = await _utilityService.GetDeviceTokenAsync(httpContext)
                        });

                        if (insertedCustomerDevice != null)
                        {
                            await _utilityService.AddDatatoQueue(insertedCustomerDevice, "report.customerdevice"
                                , new Dictionary<string, object> { { "customerdevice", "new" } });
                            customerDeviceList.Add(insertedCustomerDevice);
                            insertedCustomer.CustomerDevices = customerDeviceList;
                        }
                    }
                }
            }

            return insertedCustomer;
        }
        public async Task<LoginToken> Login(LoginDTO loginDTO, HttpContext httpContext)
        {
            var loginToken = new LoginToken();
            try
            {
                var existingCustomer = new Customers();
                existingCustomer = await _customerRepository.GetByColumnsFirstOrDefault(new Dictionary<string, object>
                {
                    { "EmailAddress", loginDTO.EmailAddress },
                    { "Password", _utilityService.HashPassword(loginDTO.Password) },
                    { "IsActive", true },
                    { "IsDeleted", false },
                });

                if (existingCustomer != null)
                {
                    var existingDevice = await _customerDeviceRepository.GetByColumnsFirstOrDefault(new Dictionary<string, object>
                    {
                        { "DeviceId", loginDTO.DeviceId },
                        { "CustomerId", existingCustomer.Id },
                        { "IsActive", true },
                        { "IsDeleted", false },
                    });
                    loginToken.Token = _utilityService.GenerateJwtToken(existingCustomer);
                    loginToken.EmailAddress = existingCustomer.EmailAddress;

                    if (existingDevice == null)
                    {
                        var insertedCustomerDevice = new CustomerDevices();
                        insertedCustomerDevice = await _customerDeviceRepository.Insert(new CustomerDevices()
                        {
                            CreatedIP = _utilityService.GetClientIP(),
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = existingCustomer.Id,
                            CustomerId = existingCustomer.Id,
                            DeviceId = loginDTO.DeviceId,
                            UserAgent = _utilityService.GetUserAgent(),
                            DeviceToken = await _utilityService.GetDeviceTokenAsync(httpContext)
                        });
                        if (insertedCustomerDevice != null)
                        {
                            await _utilityService.AddDatatoQueue(insertedCustomerDevice, "report.customerdevice"
                                , new Dictionary<string, object> { { "customerdevice", "new" } });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                loginToken.Message = ex.Message;
            }

            return loginToken;
        }
    }
}