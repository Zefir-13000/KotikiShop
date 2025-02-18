using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotikiShop.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CartId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public float UnitPrice { get; set; }

        [ForeignKey("CartId")]
        [ValidateNever]
        public virtual Cart? Cart { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public virtual Cat? Product { get; set; }
    }
}