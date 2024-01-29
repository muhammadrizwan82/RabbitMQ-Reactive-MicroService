﻿using System.ComponentModel.DataAnnotations;

namespace ReactiveMicroService.CustomerService.API.DTO
{
    public class CustomerDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CustomerId { get; set; }

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
}
