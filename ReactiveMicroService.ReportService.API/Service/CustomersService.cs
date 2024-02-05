
 
using ReactiveMicroService.ReportService.API.Models;
using ReactiveMicroService.ReportService.API.Repository;
 

namespace ReactiveMicroService.ReportService.API.Service
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
    }
}