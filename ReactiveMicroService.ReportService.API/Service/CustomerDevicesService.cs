using ReactiveMicroService.ReportService.API.Models;
using ReactiveMicroService.ReportService.API.Repository;

namespace ReactiveMicroService.ReportService.API.Service
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
