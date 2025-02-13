using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotikiShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public required string Phone {  get; set; }
        [Required]
        public required string Address { get; set; }
    }
}
