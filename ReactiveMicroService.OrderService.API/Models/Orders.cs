using System.ComponentModel.DataAnnotations.Schema;

namespace ReactiveMicroService.OrderService.API.Models
{
    //https://www.daveops.co.in/post/code-first-entity-framework-core-mysql#:~:text=NET%206.0%3A%20Code%20First%20with%20Entity%20Framework%20Core%20and%20MySQL,-Updated%3A%20Jan%2017&text=Entity%20Framework%20(EF)%20Core%20is,code%20for%20accessing%20any%20data.%20.

    [Table("Orders")]
    public class Orders : BaseModel
    {
        public string? OrderDisplayId { get; set; }

        public string? OrderDescription { get; set; }

        public double? DiscountPercentage { get; set; }

        public int? ShipmentStatusId { get; set; }

        public string? ShipmentStatus { get; set; }

        public string? ShipmentStatusDiscription { get; set; }

        public int CustomerId { get; set; }

        public int CustomerAddressId { get; set; }

        [NotMapped]
        public ICollection<OrderDetails>? OrderDetails { get; set; }

        [NotMapped]
        public double OrderTotalAmount
        {
            get
            {
                if (OrderDetails == null)
                    return 0;
                var totalAmount = OrderDetails.Sum(detail => detail.ProductDiscountedAmount) ;
                return Convert.ToDouble(totalAmount);
            }
        }

        [NotMapped]
        public double OrderTotalDiscountedAmount
        {
            get
            {
                if (OrderDetails == null)
                    return 0;
                var totalAmount = OrderDetails.Sum(detail => detail.ProductDiscountedAmount) * (DiscountPercentage > 0 ? DiscountPercentage / 100 : 1);
                return Convert.ToDouble(totalAmount);
            }
        }

        [NotMapped]
        public int OrderProductCount
        {
            get
            {
                if (OrderDetails == null)
                    return 0;

                return OrderDetails.GroupBy(detail => detail.ProductId).Count();
            }
        }

        [NotMapped]
        public int OrderItemCount
        {
            get
            {
                if (OrderDetails == null)
                    return 0;

                return OrderDetails.Sum(x=>x.ProductQuantity);
            }
        }
    }
}
