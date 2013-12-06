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
            double num;
            var errorMsg = "The " + validationContext.DisplayName + " field must be a positive number.";
            if (value == null)
                return new ValidationResult(errorMsg);
            if (!(value is sbyte && (sbyte)value >= 0
                || value is byte && (byte)value >= 0
                || value is short && (short)value >= 0
                || value is ushort && (ushort)value >= 0
                || value is int && (int)value >= 0
                || value is uint && (uint)value >= 0
                || value is long && (long)value >= 0
                || value is ulong && (ulong)value >= 0
                || value is float && (float)value >= 0
                || value is double && (double)value >= 0
                || value is decimal && (decimal)value >= 0))
                return new ValidationResult(errorMsg);
            return ValidationResult.Success;
        }
    }
}
