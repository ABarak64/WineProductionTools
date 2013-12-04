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
    public class MustNotOverflowTankAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var errorMsg = "The tank you are filling cannot hold this many more gallons.";

            if (value == null)
                return ValidationResult.Success;
            var tankToTarget = ((TankTransferDto)validationContext.ObjectInstance).ToId;
            // Only test overflow when transferring to a tank, not when emptying.
            if (tankToTarget == 0)
            {
                return ValidationResult.Success;
            }
            var gallons = (decimal)value;
            var mgr = new TankManager();
            if (mgr.TankWillOverflow(gallons, tankToTarget))
            {
                return new ValidationResult(errorMsg);
            }
            return ValidationResult.Success;
        }
    }
}
