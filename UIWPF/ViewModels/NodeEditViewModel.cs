using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VortexPings.ValidationRules;

namespace UIWPF.ViewModels
{
    public class NodeEditViewModel : FixedDialogBaseViewModel
    {
        private readonly UniqueNameValidationRule _uniqueNameValidationRule;
        private readonly IPaddressHostValidationRule _IPaddressHostValidationRule;
        private readonly PacketSizeValidationRule _packetSizeValidationRule;
        private readonly TTLValidationRule _tTLValidationRule;
        private readonly TimeOutValidationRule _timeOutValidationRule;
        private readonly WarningTimeValidatonRule _warningTimeValidatonRule;
        private readonly PingRepeatTimeValidationRule _pingRepeatValidationRule;
        private List<string> _nodeNames;

        public NodeDataViewModel NodeDataViewModel { get; private set; }

        public NodeEditViewModel(UniqueNameValidationRule uniqueNameValidationRule, 
            IPaddressHostValidationRule ipaddressHostValidationRule, 
            PacketSizeValidationRule packetSizeValidationRule, TTLValidationRule tTLValidationRule, 
            TimeOutValidationRule timeOutValidationRule, WarningTimeValidatonRule warningTimeValidatonRule, PingRepeatTimeValidationRule pingRepeatTimeValidationRule)
        {
            _uniqueNameValidationRule = uniqueNameValidationRule;
            _IPaddressHostValidationRule = ipaddressHostValidationRule;
            _packetSizeValidationRule = packetSizeValidationRule;
            _tTLValidationRule = tTLValidationRule;
            _timeOutValidationRule = timeOutValidationRule;
            _warningTimeValidatonRule = warningTimeValidatonRule;
            _pingRepeatValidationRule = pingRepeatTimeValidationRule;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            NodeDataViewModel = parameters.GetValue<NodeDataViewModel>("NodeDataViewModel");
            _nodeNames = parameters.GetValue<List<string>>("NodeNames");
            RaisePropertyChanged(nameof(NodeDataViewModel));
            
            NodeDataViewModel.PropertyChanged += NodeDataViewModel_PropertyChanged;

            ValidateNodeName();
            ValidateHostOrIPaddress();
            ValidatePackageSize();
            ValidateTTL();
            ValidateTimeOut();
            ValidateWarningTime();
            ValidatePingRepeatTime();

        }

        #region Validation
        private void NodeDataViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
       {
            var propertyName = e.PropertyName;

            switch (propertyName)
            {
                case "NodeName":
                    ValidateNodeName();
                    break;
                case "HostOrIPaddress":
                    ValidateHostOrIPaddress();
                    break;
                case "PackageSize":
                    ValidatePackageSize();
                    break;
                case "TTL":
                    ValidateTTL();
                    break;
                case "TimeOut":
                    ValidateTimeOut();
                    break;
                case "WarningTime":
                    ValidateWarningTime();
                    break;
                case "PingRepeatTime":
                    ValidatePingRepeatTime();
                    break;
            }

        }

        private void ValidatePingRepeatTime()
        {
            var value = NodeDataViewModel.PingRepeatTime;
            var propertyName = "PingRepeatTime";
            var validationResult = _pingRepeatValidationRule.Validate(value, propertyName);
            UpdateErrorList(propertyName, validationResult);
        }

        private void ValidateWarningTime()
        {
            var value = NodeDataViewModel.WarningTime;
            var propertyName = "WarningTime";
            var validationResult = _warningTimeValidatonRule.Validate(value, propertyName);
            UpdateErrorList(propertyName, validationResult);
        }

        private void ValidateTimeOut()
        {
            var value = NodeDataViewModel.TimeOut;
            var propertyName = "TimeOut";
            var validationResult = _timeOutValidationRule.Validate(value, propertyName);
            UpdateErrorList(propertyName, validationResult);
        }

        private void ValidateTTL()
       {
            var value = NodeDataViewModel.TTL;
            var propertyName = "TTL";
            var validationResult = _tTLValidationRule.Validate(value, propertyName);
            UpdateErrorList(propertyName, validationResult);
        }

        private void ValidatePackageSize()
        {
            var value = NodeDataViewModel.PackageSize;
            var propertyName = "PackageSize";
            var validationResult = _packetSizeValidationRule.Validate(value, propertyName);
            UpdateErrorList(propertyName, validationResult);
        }

        private void ValidateHostOrIPaddress()
        {
            var value = NodeDataViewModel.HostOrIPaddress;
            var propertyName = "HostOrIPaddress";
            var validationResult = _IPaddressHostValidationRule.Validate(value, propertyName);
            UpdateErrorList(propertyName, validationResult);
        }


        public void ValidateNodeName()
        {
            if (_nodeNames == null)
                _nodeNames = new List<string>();
            var value = NodeDataViewModel.NodeName;
            var propertyName = "NodeName";

            Predicate<object> isExist = (object x) => _nodeNames.Contains(value.ToString());
            var validationResult = _uniqueNameValidationRule.Validate(value, propertyName, isExist);
            UpdateErrorList(propertyName, validationResult);
        }

        private void UpdateErrorList(string propertyName, ValidationResult validationResult)
        {
            if (validationResult.IsValid == false)
            {
                NodeDataViewModel.AddError(validationResult);
            }
            else
            {
                NodeDataViewModel.RemoveError(propertyName);
            }
        }

        #endregion
        protected override void CloseDialog(string parameter)
        {
            DialogParameters = new DialogParameters { { "NodeDataViewModel", NodeDataViewModel } };
            NodeDataViewModel.PropertyChanged -= NodeDataViewModel_PropertyChanged;
            base.CloseDialog(parameter);

        }
    }
}
