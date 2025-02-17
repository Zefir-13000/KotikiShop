using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotikiShop.Models.ViewModels
{
    public class CatVM
    {
        public IEnumerable<CatFamily>? catFamilies = new List<CatFamily>();
        public int Id { get; set; }
        [Required]
        [MaxLength(120)]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        public float? Price { get; set; }
        [Required]
        public DateOnly Birthday { get; set; }
        [Required]
        public CatGender Gender { get; set; }
        public string? ImageUrl { get; set; }
        public int? CatFamilyId { get; set; }
        [ForeignKey("CatFamilyId")]
        [ValidateNever]
        public virtual CatFamily? CatFamily { get; set; }
    }
}
