using System.ComponentModel.DataAnnotations;

namespace ReactiveMicroService.OrderService.API.DTO
{
    public class OrderDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CustomerAddressId { get; set; }

        [Required]        
        public string OrderDescription { get; set; }

        [Required]        
        public List<OrderDetail> OrderDetail { get; set; }

        [Required]
        public double DiscountPercentage { get; set; }
    }
    public class OrderDetail {

        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Required]
        [Range(int.MinValue, int.MaxValue)]
        public int ProductQuantity { get; set; }

        [Required]
        [Range(double.MinValue, double.MaxValue)]
        public double UnitProductPrice { get; set; }


        [Required]
        [Range(double.MinValue, double.MaxValue)]
        public double UnitProductDiscount { get; set; }

    }

    public class LoginToken
    {
        public string? EmailAddress { get; set; }

        public string? Token { get; set; }

        public string? Message { get; set; }
    }
}
