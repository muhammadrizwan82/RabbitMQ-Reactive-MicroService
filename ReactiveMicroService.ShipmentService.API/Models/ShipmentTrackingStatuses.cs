using System.ComponentModel.DataAnnotations.Schema;

namespace ReactiveMicroService.ShipmentService.API.Models
{
    [Table("ShipmentTrackingStatuses")]
    public class ShipmentTrackingStatuses : BaseModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}