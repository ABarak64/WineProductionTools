using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WineProdTools.Data.DtoModels;
using WineProdTools.Data.Managers;

namespace WineProdTools.Data.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FromTankMustContainAtLeastThisMuchAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var errorMsg = "The tank you are transfering from does not have this many gallons to empty.";

            if (value == null)
                return ValidationResult.Success;
            var tankFromTarget = ((TankTransferDto)validationContext.ObjectInstance).FromId;
            // Only test overflow when transferring to a tank, not when emptying.
            if (tankFromTarget == 0)
            {
                return ValidationResult.Success;
            }
            var gallons = (decimal)value;
            var mgr = new TankManager();
            if (!mgr.TankContainsAtLeastThisManyGallons(gallons, tankFromTarget))
            {
                return new ValidationResult(errorMsg);
            }
            return ValidationResult.Success;
        }
    }
}
