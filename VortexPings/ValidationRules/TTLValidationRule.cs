﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VortexPings.ValidationRules
{
    public class TTLValidationRule : IValidationRule
    {
        public ValidationResult Validate(object value, string? propertyName = null, Predicate<object>? predicate = null)
        {
            if(value==null)
            {
                return new ValidationResult(false, "Value must be greater than zero!", propertyName);
            }
            var strValue = value.ToString();

            var isInt = int.TryParse(strValue, out int intValue);

            if (isInt == false)
            {
                return new ValidationResult(false, "Value must be integer!", propertyName);
            }

            if (intValue <= 0)
            {
                return new ValidationResult(false, "Value must be greater than zero!", propertyName);
            }

            if (intValue > 999)
            {
                return new ValidationResult(false, "Value too big!", propertyName);
            }

            return new ValidationResult(true, null, propertyName);
        }
    }
    
}
