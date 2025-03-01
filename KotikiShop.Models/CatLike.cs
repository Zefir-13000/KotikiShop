using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotikiShop.Models
{
    public class CatLike
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CatId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
