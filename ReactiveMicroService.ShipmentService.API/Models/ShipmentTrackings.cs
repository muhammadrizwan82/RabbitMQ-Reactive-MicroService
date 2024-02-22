using System.ComponentModel.DataAnnotations.Schema;

namespace ReactiveMicroService.ShipmentService.API.Models
{
    [Table("ShipmentTrackings")]
    public class ShipmentTrackings : BaseModel
    {
        public int ShipmentId { get; set; }
        public int ShipmentTrackingStatusId { get; set; }
        public string? TrackingComment { get; set; }

        [NotMapped]
        [ForeignKey("ShipmentId")]        
        public virtual Shipments Shipments { get; set; }
    }
}
