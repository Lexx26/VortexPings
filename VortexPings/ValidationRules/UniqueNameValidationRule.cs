using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VortexPings.ValidationRules
{
    public class UniqueNameValidationRule : IValidationRule
    {
        public ValidationResult Validate(object value, string? propertyName = null, Predicate<object>? predicate = null)
        {
            if (predicate == null||value==null)
                return null;

            var strValue = value as string;

            if(predicate(value))
            {
                return new ValidationResult(false, "Name already exists!",propertyName);
            }

            if(string.IsNullOrEmpty(strValue))
            {
                return new ValidationResult(false, "Name cannot be empty!", propertyName);
            }

            if(string.IsNullOrWhiteSpace(strValue))
            {
                return new ValidationResult(false, "Name cannot be white-space!", propertyName);
            }

            if(strValue.Length>50)
            {
                return new ValidationResult(false, "Name too long!", propertyName);
            }

            return new ValidationResult(true, null, propertyName);
        }
    }
}
