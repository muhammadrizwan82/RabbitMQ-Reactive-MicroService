
using ReactiveMicroService.ReportService.API.Models;
using ReactiveMicroService.ReportService.API.Repository;


namespace ReactiveMicroService.ReportService.API.Service
{
    public class OrdersService : GenericService<Orders>
    {
        private readonly IGenericRepository<Orders> _orderRepository;
        private readonly IGenericRepository<OrderDetails> _orderDetailRepository;
        private readonly UtilityService _utilityService;

        public OrdersService(IGenericRepository<Orders> orderRepository, IGenericRepository<OrderDetails> orderDetailRepository
            , UtilityService utilityService) : base(orderRepository, utilityService)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _utilityService = utilityService;
        }
    }
}