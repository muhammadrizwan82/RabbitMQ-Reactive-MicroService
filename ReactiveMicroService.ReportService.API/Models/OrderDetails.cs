using System.ComponentModel.DataAnnotations.Schema;

namespace ReactiveMicroService.ReportService.API.Models
{
    [Table("OrderDetails")]
    public class OrderDetails : BaseModel
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public double ProductAmount { get; set; }
        public double ProductDiscount { get; set; }
        public double ProductDiscountedAmount { get; set; }

        [NotMapped]
        [ForeignKey("OrderId")]
        // Navigation property for the order
        public virtual Orders Order { get; set; }
    }
}
