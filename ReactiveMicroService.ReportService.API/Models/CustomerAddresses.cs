using System.ComponentModel.DataAnnotations.Schema;

namespace ReactiveMicroService.ReportService.API.Models
{
    [Table("CustomerAddresses")]
    public class CustomerAddresses : BaseModel
    {
        public int CustomerId { get; set; } 
        public string? Country { get; set; }
        public string? CountryIso2Code { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Town { get; set; }
        public string? Area { get; set; }
        public string? Sector { get; set; }
        public string? Lane { get; set; }
        public string? HouseNumber { get; set; }

        [NotMapped]
        [ForeignKey("CustomerId")] 
        public virtual Customers? Customer { get; set; }
    }
}
