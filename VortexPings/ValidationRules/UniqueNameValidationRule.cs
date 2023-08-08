using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VortexPings.ValidationRules
{
    public class UniqueNameValidationRule : IValidationRule
    {
        public ValidationResult Validate(object value, string? propertyName = null, Predicate<object>? isUnique = null)
        {
            if (isUnique == null||value==null)
                return null;

            var strValue = value as string;

            
        }
    }
}
