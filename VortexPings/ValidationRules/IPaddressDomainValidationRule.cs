using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VortexPings.ValidationRules
{
    public class IPaddressHostValidationRule : IValidationRule
    {
        public ValidationResult Validate(object value, string? propertyName = null, Predicate<object>? predicate = null)
        {
            var strValue = value as string;

            if (string.IsNullOrEmpty(strValue))
            {
                return new ValidationResult(false, "Value cannot be empty!", propertyName);
            }

            if (string.IsNullOrWhiteSpace(strValue))
            {
                return new ValidationResult(false, "Value cannot be white-space!", propertyName);
            }

            if (strValue.Length > 254)
            {
                return new ValidationResult(false, "Value too long!", propertyName);
            }

            return new ValidationResult(true, null, propertyName);
        }
    }
}
