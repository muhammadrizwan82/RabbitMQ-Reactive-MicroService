
using ReactiveMicroService.CustomerService.API.DTO;
using ReactiveMicroService.CustomerService.API.Models;
using ReactiveMicroService.CustomerService.API.Repository;

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
            if ((await _customerRepository.GetByColumns(new Dictionary<string, object>
            {
                { "EmailAddress", customerDTO.EmailAddress }
            })).Count == 0)
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
                });
                if (insertedCustomer != null)
                {
                    var existingDevice = await _customerDeviceRepository.GetByColumns(new Dictionary<string, object>
                    {
                        { "DeviceId", customerDTO.DeviceId },
                        { "CustomerId", insertedCustomer.Id }
                    });
                    if (existingDevice.Count == 0)
                    {
                        var insertedCustomerDevice = new CustomerDevices();
                        var customerDeviceList = new List<CustomerDevices>();
                        insertedCustomerDevice = await _customerDeviceRepository.Insert(new CustomerDevices()
                        {
                            CreatedIP = _utilityService.GetClientIP(),
                            CreatedBy = insertedCustomer.Id,
                            CustomerId = insertedCustomer.Id,
                            DeviceId = customerDTO.DeviceId,
                            UserAgent = _utilityService.GetUserAgent(),
                            DeviceToken = await _utilityService.GetDeviceTokenAsync(httpContext)
                        }); ;

                        if (insertedCustomerDevice != null)
                        {
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
                var insertedCustomer = new Customers();
                insertedCustomer = await _customerRepository.GetByColumnsFirstOrDefault(new Dictionary<string, object>
                {
                    { "EmailAddress", loginDTO.EmailAddress },
                    { "Password", _utilityService.HashPassword(loginDTO.Password) }
                });

                if (insertedCustomer != null)
                {
                    var existingDevice = await _customerDeviceRepository.GetByColumnsFirstOrDefault(new Dictionary<string, object>
                    {
                        { "DeviceId", loginDTO.DeviceId },
                        { "CustomerId", insertedCustomer.Id }
                    });

                    loginToken.Token = _utilityService.GenerateJwtToken(insertedCustomer);
                    loginToken.EmailAddress = insertedCustomer.EmailAddress;
                    if (existingDevice != null)
                    {
                        var insertedCustomerDevice = new CustomerDevices();
                        var customerDeviceList = new List<CustomerDevices>();
                        insertedCustomerDevice = await _customerDeviceRepository.Insert(new CustomerDevices()
                        {
                            CreatedIP = _utilityService.GetClientIP(),
                            CreatedBy = insertedCustomer.Id,
                            CustomerId = insertedCustomer.Id,
                            DeviceId = loginDTO.DeviceId,
                            UserAgent = _utilityService.GetUserAgent(),
                            DeviceToken = await _utilityService.GetDeviceTokenAsync(httpContext)
                        });                         
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return loginToken;
        }
    }
}