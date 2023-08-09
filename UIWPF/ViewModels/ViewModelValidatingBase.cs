using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VortexPings.ValidationRules;

namespace UIWPF.ViewModels
{
    public class ViewModelValidatingBase:BindableBase, INotifyDataErrorInfo
    {
        #region Validation

        private List<ValidationResult> _errors = new List<ValidationResult>();

        private bool _canSave;
        public bool CanSave
        {
            get { return _canSave; }
            set { SetProperty(ref _canSave, value); }
        }

        public bool HasErrors { get { CanSave = !_errors.Any(); return _errors.Any(); } }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (_errors.Any(t => t.PropertyName == propertyName))
            {
                var result = _errors.Where(t => t.PropertyName == propertyName).Select(t => t.ErrorMessage);

                return result;
            }

            return null;
        }

        public void RemoveError(string propertyName)
        {
            var errorToRemove = _errors.FirstOrDefault(t => t.PropertyName == propertyName);
            _errors.Remove(errorToRemove);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public void AddError(ValidationResult validationResult)
        {
            if (!_errors.Any(t => t.PropertyName == validationResult.PropertyName))
            {
                _errors.Add(validationResult);
            }
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(validationResult.PropertyName));
        }
        #endregion
    }
}
