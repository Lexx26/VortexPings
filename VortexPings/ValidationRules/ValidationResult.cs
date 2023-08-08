using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VortexPings.ValidationRules
{
    public class ValidationResult
    {
        public ValidationResult(bool isValid, string errorMessage, string propertyName)
        {
            IsValid = isValid;
            ErrorMessage=errorMessage;
            PropertyName = propertyName;
        }

        public bool IsValid { get; private set; }

        public string ErrorMessage { get;private set; }

        public string PropertyName { get; private set; }
    }
}
