using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIWPF.ViewModels
{
    public class WarningDialogViewModel: FixedDialogBaseViewModel
    {
        private string _message1;
        public string Message1
        {
            get { return _message1; }
            set { SetProperty(ref _message1, value); }
        }

        private string _message2;
        public string Message2
        {
            get { return _message2; }
            set { SetProperty(ref _message2, value); }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            Message1 = parameters.GetValue<string>("Message1");
            Message2 = parameters.GetValue<string>("Message2");
        }
    }
}
