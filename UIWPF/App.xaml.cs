using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UIWPF.ViewModels;
using UIWPF.Views;
using VortexPings.Factories;
using VortexPings.ValidationRules;

namespace UIWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<NodeDataFactory, NodeDataFactory>();
            containerRegistry.RegisterSingleton<NodeFactory, NodeFactory>();

            //ValidationRules
            containerRegistry.RegisterSingleton<UniqueNameValidationRule, UniqueNameValidationRule>();
            containerRegistry.RegisterSingleton<IPaddressHostValidationRule, IPaddressHostValidationRule>();
            containerRegistry.RegisterSingleton<PacketSizeValidationRule, PacketSizeValidationRule>();
            containerRegistry.RegisterSingleton<TTLValidationRule, TTLValidationRule>();
            containerRegistry.RegisterSingleton<WarningTimeValidatonRule, WarningTimeValidatonRule>();
            containerRegistry.RegisterSingleton<TimeOutValidationRule, TimeOutValidationRule>();

            //Navigation
            containerRegistry.RegisterForNavigation<PingGridView>();

            //Dialogs
            containerRegistry.RegisterDialog<NodeGroupEditView, NodeGroupEditViewModel>();
            containerRegistry.RegisterDialog<NodeGroupDetailView, NodeGroupDetailViewModel>();
            containerRegistry.RegisterDialog<NodeEditView, NodeEditViewModel>();
            containerRegistry.RegisterDialog<NodeDetailView, NodeDetailViewModel>();
            containerRegistry.RegisterDialogWindow<FixedDialogWindow>("FixedDialogWindow");
            
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule(typeof(MainModule));
        }

        protected override Window CreateShell()
        {
            var window = Container.Resolve<MainWindow>();
            return window;
        }
    }
}
