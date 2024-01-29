using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReactiveMicroService.CustomerService.API.Models
{
    public class BaseModel : IModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
