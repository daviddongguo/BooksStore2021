﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksStore2021.Domain.Entities
{
    public class Order
    {
        [Key]
        public long Id { get; set; }


        [Required]
        public long ShoppingCartId { get; set; }
        [ForeignKey("ShoppingCartId")]
        public virtual ShoppingCart ShoppingCart { get; set; }

        [Required]
        public long ShippingDetailsId { get; set; }
        [ForeignKey("ShippingDetailsId")]
        public virtual ShippingDetails ShippingDetails { get; set; }


        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime ShippingDate { get; set; }
        public string OrderStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; }

    }
}
