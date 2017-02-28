using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.MailRuLinkCreator.MainViewModel;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace AM.MailRuLinkCreator.MainView
{
    public class MainModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly NavigationService _navigationService;

        public MainModule(IRegionManager regionManager, NavigationService navigationService)
        {
            this._regionManager = regionManager;
            _navigationService = navigationService;
        }

        public void Initialize()
        {
            ApplicationContext.Container.RegisterType<object, MainView>(Views.MainView);
            ApplicationContext.Container.RegisterType<MainViewModel.MainViewModel>();
            _navigationService.NavigateToRegion(Regions.MainViewRegion, Views.MainView);
        }
    }
}
