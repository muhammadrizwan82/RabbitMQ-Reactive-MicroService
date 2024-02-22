using System.ComponentModel.DataAnnotations.Schema;

namespace ReactiveMicroService.ShipmentService.API.Models
{
    [Table("Shipments")]
    public class Shipments : BaseModel
    {
        public string OrderDisplayId { get; set; }
        public int OrderId { get; set; }
        public int CustomerAddressId { get; set; }
        public int ShipmentStatusId { get; set; }

        [NotMapped]
        public ICollection<ShipmentTrackings>? ShipmentTrackings { get; set; }
    }
}
