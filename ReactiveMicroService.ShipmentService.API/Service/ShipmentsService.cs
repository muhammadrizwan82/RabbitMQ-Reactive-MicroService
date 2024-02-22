using ReactiveMicroService.ShipmentService.API.DTO;
using ReactiveMicroService.ShipmentService.API.Models;
using ReactiveMicroService.ShipmentService.API.Repository;
using static ReactiveMicroService.ShipmentService.API.Enums.Enum;

namespace ReactiveMicroService.ShipmentService.API.Service
{
    public class ShipmentsService : GenericService<Shipments>
    {
        private readonly IGenericRepository<Shipments> _shipmentRepository;
        private readonly IGenericRepository<ShipmentTrackings> _shipmentTrackingRepository;
        private readonly UtilityService _utilityService;

        public ShipmentsService(
            IGenericRepository<Shipments> shipmentRepository,
            IGenericRepository<ShipmentTrackings> shipmentTrackingRepository,
            UtilityService utilityService) : base(shipmentRepository, utilityService)
        {           
            _utilityService = utilityService;
            _shipmentRepository = shipmentRepository;
            _shipmentTrackingRepository = shipmentTrackingRepository;
        }

        public async Task<Shipments> CreateShipmentAsync(ShipmentDTO shipmentDTO, HttpContext httpContext)
        {
            var insertedShipment = new Shipments();
            if (await _shipmentRepository.GetByColumnsFirstOrDefault(new Dictionary<string, object>
            {
                { "OrderId", shipmentDTO.OrderId }
            }) == null)
            {
                insertedShipment = await _shipmentRepository.Insert(new Shipments()
                {
                    OrderDisplayId = shipmentDTO.OrderDisplayId,
                    OrderId = shipmentDTO.OrderId,
                    ShipmentStatusId= (int)ShipmentStatus.OrderReceived,
                    CustomerAddressId = shipmentDTO.CustomerAddressId,                    
                    CreatedIP = _utilityService.GetClientIP(),                    
                    CreatedBy = shipmentDTO.CustomerId,
                    CreatedAt = DateTime.UtcNow
                });
                if (insertedShipment != null)
                {
                    await _utilityService.AddDatatoQueue(insertedShipment, "report.shipment"
                        , new Dictionary<string, object> { { "shipment", "new" } });

                    var insertedShipmentTracking = new ShipmentTrackings();
                    insertedShipmentTracking = await _shipmentTrackingRepository.Insert(new ShipmentTrackings()
                    {
                        CreatedIP = _utilityService.GetClientIP(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = shipmentDTO.CustomerId,
                        ShipmentId = insertedShipment.Id,
                        ShipmentTrackingStatusId = (int)ShipmentTrackingStatus.ShipmentPending,
                        TrackingComment = "Order Created in Shipment System"
                    });

                    if (insertedShipmentTracking != null)
                    {
                        insertedShipment.ShipmentTrackings.Add(insertedShipmentTracking);
                        await _utilityService.AddDatatoQueue(insertedShipmentTracking, "report.shipmenttracking"
                            , new Dictionary<string, object> { { "shipmenttracking", "new" } });
                         
                    }
                }
            }

            return insertedShipment;
        }
    }
}