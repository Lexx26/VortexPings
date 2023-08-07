using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIWPF.ViewModels
{
    public class NodeGroupEditViewModel : FixedDialogBaseViewModel
    {

        private string _groupName;
        public string GroupName
        {
            get { return _groupName; }
            set { SetProperty(ref _groupName, value); }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            GroupName = parameters.GetValue<string>("GroupName");
        }

        protected override void CloseDialog(string parameter)
        {
            DialogParameters = new DialogParameters { { "GroupName", GroupName } };
            base.CloseDialog(parameter);

        }


    }
}
