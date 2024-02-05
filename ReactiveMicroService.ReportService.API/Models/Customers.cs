using System.ComponentModel.DataAnnotations.Schema;

namespace ReactiveMicroService.ReportService.API.Models
{
    [Table("Customers")]
    public class Customers : BaseModel
    {                
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public int? DialCode { get; set; }
        public string? PhoneNumber { get; set; }

        [NotMapped]
        public ICollection<CustomerDevices>? CustomerDevices { get; set; }
    }
}
