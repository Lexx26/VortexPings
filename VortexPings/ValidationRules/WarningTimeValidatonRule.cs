using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VortexPings.ValidationRules
{
    public class WarningTimeValidatonRule:IValidationRule
    {
        public ValidationResult Validate(object value, string? propertyName = null, Predicate<object>? predicate = null)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Value must be greater than zero!", propertyName);
            }
            var strValue = value.ToString();

            var isInt = int.TryParse(strValue, out int intValue);

            if (isInt == false)
            {
                return new ValidationResult(false, "Value must be integer!", propertyName);
            }

            if (intValue <= 10)
            {
                return new ValidationResult(false, "Value must be greater than 10!", propertyName);
            }

            if (intValue > 99999)
            {
                return new ValidationResult(false, "Value too big!", propertyName);
            }

            return new ValidationResult(true, null, propertyName);
        }
    }
}
