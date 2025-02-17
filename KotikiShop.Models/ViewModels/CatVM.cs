using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace KotikiShop.Models.ViewModels
{
    public class RangeClusiveAttribute : ValidationAttribute
    {
        public double Minimum { get; set; }

        public bool MinimumExclusive { get; set; }

        public double Maximum { get; set; }

        public bool MaximumExclusive { get; set; }

        public RangeClusiveAttribute()
        {
            Minimum = 0;
            MinimumExclusive = false;
            Maximum = double.MaxValue;
            MaximumExclusive = false;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            double convertedValue;

            try
            {
                convertedValue = Convert.ToDouble(value);
            }
            catch (Exception exception) when (exception is FormatException || exception is InvalidCastException || exception is OverflowException)
            {
                return false;
            }

            if (MinimumExclusive && MaximumExclusive)
            {
                return Minimum < convertedValue && convertedValue < Maximum;
            }
            else if (!MinimumExclusive && MaximumExclusive)
            {
                return Minimum <= convertedValue && convertedValue < Maximum;
            }
            else if (MinimumExclusive && !MaximumExclusive)
            {
                return Minimum < convertedValue && convertedValue <= Maximum;
            }

            return Minimum <= convertedValue && convertedValue <= Maximum;
        }

        public override string FormatErrorMessage(string name)
        {
            string minimumClusivity = MinimumExclusive ? "exclusively" : "inclusively";
            string maximumClusivity = MaximumExclusive ? "exclusively" : "inclusively";
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Minimum, minimumClusivity, Maximum, maximumClusivity);
        }
    }

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
        [Range(0.00001, 100, ErrorMessage = "Price must be in range 0.0001 to 100")]
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
