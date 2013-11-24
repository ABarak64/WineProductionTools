using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WineProdTools.Data.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PositiveNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var errorMsg = "The " + validationContext.DisplayName + " field must be a positive number.";
            if (value == null)
                return new ValidationResult(errorMsg);
            if (!(value is decimal))
                return new ValidationResult(errorMsg);
            if ((decimal)value <= 0)
                return new ValidationResult(errorMsg);
            return ValidationResult.Success;
        }
    }
}
