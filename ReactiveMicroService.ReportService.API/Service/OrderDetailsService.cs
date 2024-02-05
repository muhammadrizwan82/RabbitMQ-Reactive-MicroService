using ReactiveMicroService.ReportService.API.Models;
using ReactiveMicroService.ReportService.API.Repository; 

namespace ReactiveMicroService.ReportService.API.Service
{
    public class OrderDetailsService : GenericService<OrderDetails>
    {
        private readonly IGenericRepository<OrderDetails> _orderDetailRepository;
        private readonly UtilityService _utilityService;
        public OrderDetailsService(IGenericRepository<OrderDetails> orderDetailRepository, UtilityService utilityService) : base(orderDetailRepository, utilityService)
        {
            _orderDetailRepository = orderDetailRepository;
            _utilityService = utilityService;
        }
    }
}
