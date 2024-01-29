namespace ReactiveMicroService.OrderService.API.Enums
{
    public class Enum
    {
        public enum OrderStatus 
        {
            OrderPending =1,
            OrderPaid = 2,
            OrderShipmentDeliveryPending = 3,
            OrderShipmentDeliveryInprogress = 4,
            OrderShipmentDeliveryWrongAddress = 5,
            OrderShipmentDeliveryDone = 6,
            OrderShipmentDeliveryReturn = 7,
            OrderRefund = 8,
            OrderCancel = 9
        }
    }
}
