 
using ReactiveMicroService.ReportService.API.Models;
using ReactiveMicroService.ReportService.API.Repository;

namespace ReactiveMicroService.ReportService.API.Service
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

         
    }
}
