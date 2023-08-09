using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIWPF.ViewModels
{
    public class NodeEditViewModel : FixedDialogBaseViewModel
    {
        public NodeDataViewModel NodeDataViewModel { get; private set; }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            base.OnDialogOpened(parameters);
            NodeDataViewModel = parameters.GetValue<NodeDataViewModel>("NodeDataViewModel");
            RaisePropertyChanged(nameof(NodeDataViewModel));
        }

        protected override void CloseDialog(string parameter)
        {
            DialogParameters = new DialogParameters { { "NodeDataViewModel", NodeDataViewModel } };
            base.CloseDialog(parameter);

        }
    }
}
