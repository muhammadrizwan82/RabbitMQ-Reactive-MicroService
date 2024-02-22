using System.ComponentModel.DataAnnotations;

namespace ReactiveMicroService.ShipmentService.API.DTO
{
     
    public class ShipmentDTO
    {
        [Required(ErrorMessage = "OrderDisplayId is required")]
        [StringLength(int.MaxValue, ErrorMessage = "Must be between 1 and int.MaxValue", MinimumLength = 1)]
        [RegularExpression("^[a-zA-Z0-9-]+$")]
        public string? OrderDisplayId { get; set; }

        [Required(ErrorMessage = "OrderId is required")]
        [StringLength(int.MaxValue, ErrorMessage = "Must be between 1 and int.MaxValue", MinimumLength = 1)]        
        public int OrderId { get; set; }

        [Required(ErrorMessage = "CustomerAddressId is required")]
        [StringLength(int.MaxValue, ErrorMessage = "Must be between 1 and int.MaxValue", MinimumLength = 1)]
        public int CustomerAddressId { get; set; }

        [Required(ErrorMessage = "CustomerId is required")]
        [StringLength(int.MaxValue, ErrorMessage = "Must be between 1 and int.MaxValue", MinimumLength = 1)]
        public int CustomerId { get; set; }
    }

    public class LoginToken
    {
        public string? EmailAddress { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
    }
}
