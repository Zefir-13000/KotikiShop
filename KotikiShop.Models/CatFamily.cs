using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotikiShop.Models
{
    public class CatFamily
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(120)]
        public required string Name { get; set; }
    }
}
