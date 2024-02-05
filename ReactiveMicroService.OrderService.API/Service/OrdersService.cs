using ReactiveMicroService.OrderService.API.DTO;
using ReactiveMicroService.OrderService.API.Models;
using ReactiveMicroService.OrderService.API.Repository;
using static ReactiveMicroService.OrderService.API.Enums.Enum;

namespace ReactiveMicroService.OrderService.API.Service
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

        public async Task<Orders> CreateNewOrder(OrderDTO orderDTO, int CustomerId)
        {
            var insertedOrder = await _orderRepository.Insert(new Orders()
            {
                CustomerAddressId = orderDTO.CustomerAddressId,
                CustomerId = CustomerId,
                DiscountPercentage = orderDTO.DiscountPercentage,
                OrderDescription = orderDTO.OrderDescription,
                ShipmentStatus = "Pending",
                ShipmentStatusDiscription = "New Unpaid Order",
                ShipmentStatusId = (int)OrderStatus.OrderPending,
                CreatedBy = CustomerId,
                CreatedAt = DateTime.UtcNow,
                CreatedIP = _utilityService.GetClientIP(),
                IsActive = true,
                IsDeleted = false,
            }); ;
            if (insertedOrder != null)
            {
                insertedOrder.OrderDisplayId = $"{DateTime.UtcNow.Year}{DateTime.UtcNow.Month.ToString("00")}{DateTime.UtcNow.Day.ToString("00")}-{new Random().Next(1000, 10000)}";
                await _orderRepository.Update(insertedOrder);
                var insertedOrderDetailList = new List<OrderDetails>();
                foreach (var orderDetail in orderDTO.OrderDetail)
                {
                    var insertedOrderDetail = await _orderDetailRepository.Insert(new OrderDetails()
                    {
                        CreatedBy = insertedOrder.CustomerId,
                        CreatedAt = DateTime.UtcNow,
                        CreatedIP = _utilityService.GetClientIP(),
                        IsActive = true,
                        IsDeleted = false,
                        OrderId = insertedOrder.Id,
                        ProductAmount = orderDetail.ProductQuantity * orderDetail.UnitProductPrice,
                        ProductDiscountedAmount = orderDetail.ProductQuantity * ((orderDetail.UnitProductDiscount > 0 ?
                                (orderDetail.UnitProductDiscount / 100 * orderDetail.UnitProductPrice) : (1 * orderDetail.UnitProductPrice))),
                        ProductQuantity = orderDetail.ProductQuantity,
                        ProductDiscount = orderDetail.UnitProductDiscount / 100,
                        ProductId = orderDetail.ProductId
                    });
                    if (insertedOrderDetail != null)
                    {
                        insertedOrderDetailList.Add(insertedOrderDetail);
                    }
                }
                insertedOrder.OrderDetails = insertedOrderDetailList;
                await _utilityService.AddDatatoQueue(insertedOrder, "report.order", new Dictionary<string, object>() {
                    { "order","new"}
                });
                return insertedOrder;
            }
            else
            {
                return null;
            }
        }
        public async Task<Orders> GetOrderSummary(int OrderId)
        {
            var order = await _orderRepository.GetById(OrderId);
            if (order != null)
            {
                var filter = new Dictionary<string, object>();
                filter.Add("OrderId", order.Id);
                order.OrderDetails = await _orderDetailRepository.GetByColumns(filter);
                return order;
            }
            else
            {
                return null;
            }

        }
    }
}