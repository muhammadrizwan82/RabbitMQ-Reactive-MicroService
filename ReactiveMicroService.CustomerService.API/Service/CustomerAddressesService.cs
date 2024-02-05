using ReactiveMicroService.CustomerService.API.DTO;
using ReactiveMicroService.CustomerService.API.Models;
using ReactiveMicroService.CustomerService.API.Repository;

namespace ReactiveMicroService.CustomerService.API.Service
{
    public class CustomerAddressesService : GenericService<CustomerAddresses>
    {
        private readonly IGenericRepository<CustomerAddresses> _customerAddressesRepository;
        private readonly UtilityService _utilityService;
        public CustomerAddressesService(
            IGenericRepository<CustomerAddresses> customerAddressesRepository,
            UtilityService utilityService) : base(customerAddressesRepository, utilityService)
        {
            _customerAddressesRepository = customerAddressesRepository;
            _utilityService = utilityService;
        }

        public async Task<CustomerAddresses> AddCustomerAddress(CustomerAddressDTO customerAddressDTO, int CustomerId)
        {
            var insertedCustomerAddress = await _customerAddressesRepository.Insert(new CustomerAddresses()
            {
                Area = customerAddressDTO.Area,
                City = customerAddressDTO.City,
                Country = customerAddressDTO.Country,
                CountryIso2Code = customerAddressDTO.CountryIso2Code,
                CreatedIP = _utilityService.GetClientIP(),                
                CreatedBy = CustomerId,
                CustomerId = CustomerId,
                HouseNumber = customerAddressDTO.HouseNumber,
                Lane = customerAddressDTO.Lane,
                Sector = customerAddressDTO.Sector,
                State = customerAddressDTO.State,
                Town = customerAddressDTO.Town,
                CreatedAt = DateTime.UtcNow,                
            });
            if (insertedCustomerAddress != null) {
                await _utilityService.AddDatatoQueue(insertedCustomerAddress, "report.customeraddress"
                        , new Dictionary<string, object> { { "customeraddress", "new" } });
            }
            return insertedCustomerAddress;
        }
    }
}
