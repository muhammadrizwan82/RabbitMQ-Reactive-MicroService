using ReactiveMicroService.OrderService.API.Models;
using ReactiveMicroService.OrderService.API.Repository;

namespace ReactiveMicroService.OrderService.API.Service
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
