using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ReactiveMicroService.ShipmentService.API.Models
{
    public class BaseModel : IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }               
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int CreatedBy { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public string? CreatedIP { get; set; }
        public int UpdatedBy { get; set; } = 0;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedIP { get; set; }
    }
}
