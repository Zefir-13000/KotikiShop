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
    public class Cart
    {
        [Key]
        public int Id { get; set; } // Primary Key
        [Required]
        public string ApplicationUserId { get; set; } // Foreign Key or identifier for the user

        [InverseProperty("Cart")]
        [ValidateNever]
        public virtual ICollection<CartItem> CartItems { get; set; } // Navigation property for related CartItems
        public float TotalPrice => (float)CartItems.Sum(item => item.Quantity * item.Product.Price);
        public int TotalItems => CartItems.Sum(item => item.Quantity);
        public Cart()
        {
            CartItems = new List<CartItem>();
        }
    }
}
