using System.ComponentModel.DataAnnotations.Schema;

namespace ReactiveMicroService.CustomerService.API.Models
{

    [Table("CustomerDevices")]
    public class CustomerDevices : BaseModel
    {        
        public int CustomerId { get; set; }        
        public string? DeviceId { get; set; }        
        public string? DeviceToken { get; set; }        
        public string? UserAgent { get; set; }

        [NotMapped]        
        [ForeignKey("CustomerId")]
        // Navigation property for the order
        public virtual Customers Customer { get; set; }
    }
}
