using Microsoft.EntityFrameworkCore;
using ReactiveMicroService.ShipmentService.API.Models;

namespace ReactiveMicroService.ShipmentService.API.Repository
{
    public class DBContext : GenericDBContext<DBContext>
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }
        public DbSet<ShipmentStatuses> ShipmentStatuses { get; set; }
        public DbSet<ShipmentTrackingStatuses> ShipmentTrackingStatuses { get; set; }
        public DbSet<Shipments> Shipments { get; set; }
        public DbSet<ShipmentTrackings> ShipmentTrackings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShipmentStatuses>().HasData(
                new ShipmentStatuses
                {
                    Title = "Order Received",
                    Description = " This stage involves the customer placing an order for a product or service through various channels such as online platforms, phone calls, or in-person transactions."
                },
                new ShipmentStatuses
                {
                    Title = "Order Processing",
                    Description = "Once the order is received, it is processed by the seller. This includes verifying the order details, checking inventory availability, and preparing the order for shipment."

                },
                new ShipmentStatuses
                {
                    Title = "Picking and Packing",
                    Description = "In this stage, the items from the order are picked from the inventory shelves and packed securely for shipping. This may involve sorting, labeling, and packaging the items appropriately."
                },
                new ShipmentStatuses
                {
                    Title = "Shipping/Carrier Selection",
                    Description = "The seller selects the shipping carrier or service to transport the package to the customer. This decision may depend on factors such as cost, delivery speed, and the destination of the package."
                },
                new ShipmentStatuses
                {
                    Title = "Transit",
                    Description = "The package is transported from the seller's location to the customer's address. During this stage, the package may undergo multiple handling processes, including loading onto trucks, transit through distribution centers, and sorting at various checkpoints."
                },
                new ShipmentStatuses
                {
                    Title = "Tracking",
                    Description = "Many shipping carriers provide tracking information that allows both the seller and the customer to monitor the progress of the package in real-time. This helps in ensuring transparency and managing expectations regarding delivery times."
                },
                new ShipmentStatuses
                {
                    Title = "Delivery",
                    Description = "The package is delivered to the customer's address. This may involve the package being handed over directly to the customer, left at a designated location, or picked up from a nearby delivery center, depending on the shipping method and recipient preferences."
                },
                new ShipmentStatuses
                {
                    Title = "Confirmation and Feedback",
                    Description = "After receiving the package, the customer confirms delivery and may provide feedback on their overall experience. This feedback can be valuable for the seller to improve their order fulfillment process and customer satisfaction levels."
                },
                new ShipmentStatuses
                {
                    Title = "Returns and Exchanges",
                    Description = "In case of any issues with the order, such as damaged items or incorrect products, the customer may initiate a return or exchange process. This involves contacting the seller, returning the item, and receiving a replacement or refund as per the seller's return policy."
                },
                new ShipmentStatuses
                {
                    Title = "Post-Delivery Support",
                    Description = "The seller may offer post-delivery support to address any further questions or concerns the customer may have regarding the received items or the order process. This can include assistance with product usage, troubleshooting, or additional services."
                }
            );

            modelBuilder.Entity<ShipmentTrackingStatuses>().HasData(
                new ShipmentTrackingStatuses
                {
                    Title = "Shipment Pending",
                    Description = "Order received but shipment is pending"
                },
                new ShipmentTrackingStatuses
                {
                    Title = "Order Processing",
                    Description = "The carrier is processing the package at one of its facilities, such as a sorting center or distribution hub"
                },
                new ShipmentTrackingStatuses
                {
                    Title = "In Transit",
                    Description = "The package is on its way to the destination and is moving through various checkpoints, such as transportation hubs and delivery centers."
                },
                new ShipmentTrackingStatuses
                {
                    Title = "Out for Delivery",
                    Description = "The package is in the final stage of transit and is being delivered to the recipient's address by a local delivery driver."
                },
                new ShipmentTrackingStatuses
                {
                    Title = "Delivered",
                    Description = "The package has been successfully delivered to the recipient's address and is in their possession."
                },
                new ShipmentTrackingStatuses
                {
                    Title = "Attempted Delivery",
                    Description = "The carrier attempted to deliver the package but was unsuccessful, often due to the recipient not being available to receive the package. In such cases, the carrier may leave a delivery attempt notice or attempt redelivery on another day."
                },
                new ShipmentTrackingStatuses
                {
                    Title = "Hold at Location",
                    Description = "The package is being held at a designated location, such as a local post office or delivery center, for the recipient to pick up."
                },
                new ShipmentTrackingStatuses
                {
                    Title = "Customs Clearance",
                    Description = "For international shipments, the package may be held at customs for inspection and clearance before it can continue its journey to the destination country."
                },
                new ShipmentTrackingStatuses
                {
                    Title = "Exception",
                    Description = "This status indicates that there is an issue or delay with the shipment, such as a delivery exception, weather delay, or incorrect address. Additional information may be provided to explain the nature of the exception"
                },
                new ShipmentTrackingStatuses
                {
                    Title = "Return to Sender"
                ,
                    Description = "The package is being returned to the sender, either due to an unsuccessful delivery attempt, an incorrect address, or the recipient refusing the delivery."
                }
            );
        }
    }
}
