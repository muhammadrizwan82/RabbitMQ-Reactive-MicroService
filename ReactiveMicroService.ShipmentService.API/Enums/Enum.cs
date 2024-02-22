namespace ReactiveMicroService.ShipmentService.API.Enums
{
    public class Enum
    {
        public enum ShipmentStatus
        {
            OrderReceived=1,
            OrderProcessing,
            PickingandPacking,
            ShippingCarrierSelection,
            Transit,
            Tracking,
            Delivery,
            ConfirmationandFeedback,
            ReturnsandExchanges,
            PostDeliverySupport,
        }
        public enum ShipmentTrackingStatus
        {
            ShipmentPending=1,
            OrderProcessing,
            InTransit,
            OutforDelivery,
            Delivered,
            AttemptedDelivery,
            HoldatLocation,
            CustomsClearance,
            Exception,
            ReturntoSender
        }
    }
}
