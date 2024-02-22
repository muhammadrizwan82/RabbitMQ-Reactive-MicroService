using System.ComponentModel.DataAnnotations.Schema;

namespace ReactiveMicroService.ShipmentService.API.Models
{
    [Table("ShipmentStatuses")]
    public class ShipmentStatuses : BaseModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}