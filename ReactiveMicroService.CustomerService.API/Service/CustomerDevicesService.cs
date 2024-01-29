using ReactiveMicroService.CustomerService.API.Models;
using ReactiveMicroService.CustomerService.API.Repository;

namespace ReactiveMicroService.CustomerService.API.Service
{
    public class CustomerDevicesService : GenericService<CustomerDevices>
    {
        private readonly IGenericRepository<CustomerDevices> _customerDevicesService;
        private readonly UtilityService _utilityService;
        public CustomerDevicesService(
            IGenericRepository<CustomerDevices> customerDevicesService, 
            UtilityService utilityService) : base(customerDevicesService, utilityService)
        {
            _customerDevicesService = customerDevicesService;
            _utilityService = utilityService;
        }
    }
}
