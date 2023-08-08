using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
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
    public class NodeGroupEditViewModel : FixedDialogBaseViewModel, INotifyDataErrorInfo
    {
       
        private IValidationRule _validationRule;
        public NodeGroupEditViewModel()
        {
            _validationRule = new UniqueNameValidationRule();
        }

        private bool _canSave;
        public bool CanSave
        {
            get { return _canSave; }
            set { SetProperty(ref _canSave, value); }
        }

        private List<string> _groupNames;

        private string _groupName;

        public string GroupName
        {
            get { return _groupName; }
            set { SetProperty(ref _groupName, value); ValidateGroupName(_groupName, nameof(GroupName)); }
        }
        #region Input validation
        private List<ValidationResult> _errors = new List<ValidationResult>();
        public bool HasErrors { get { CanSave = !_errors.Any(); return _errors.Any(); } }
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        private void ValidateGroupName(object value, string propertyName)
        {
            if (_groupNames == null)
                _groupNames = new List<string>();

            Predicate<object> isExist = (object x) => _groupNames.Contains(value.ToString());
            var validationResult = _validationRule.Validate(value, propertyName, isExist);

            if(validationResult.IsValid==false)
            {
                AddError(validationResult);
            }
            else
            {
                RemoveError(propertyName);
            }
        }

        private void RemoveError(string propertyName)
        {
            var errorToRemove = _errors.FirstOrDefault(t => t.PropertyName == propertyName);
            _errors.Remove(errorToRemove);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void AddError(ValidationResult validationResult)
        {
                if (!_errors.Any(t => t.PropertyName == validationResult.PropertyName))
                {
                    _errors.Add(validationResult);
                }
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(validationResult.PropertyName));
        }

        #endregion

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            GroupName = parameters.GetValue<string>("GroupName");
            _groupNames = parameters.GetValue<List<string>>("GroupNames");
        }

        protected override void CloseDialog(string parameter)
        {
            DialogParameters = new DialogParameters { { "GroupName", GroupName } };
            base.CloseDialog(parameter);

        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if(_errors.Any(t=>t.PropertyName==propertyName))
            {
                var result = _errors.Where(t=>t.PropertyName==propertyName).Select(t=>t.ErrorMessage);
               
                return result;
            }
           
            return null;
        }
    }
}
