using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VortexPings.ValidationRules
{
    public interface IValidationRule
    {
        ValidationResult Validate(object value, string? propertyName=null, Predicate<object>? predicate=null);
    }
}
