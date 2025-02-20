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
    public class CatComment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }
        [Required]
        public int CatId { get; set; }
        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public virtual ApplicationUser? User { get; set; }
    }
}
