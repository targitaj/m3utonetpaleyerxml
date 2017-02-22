using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using AM.MailRuLinkCreator.MainViewModel;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace AM.MailRuLinkCreator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IUnityContainer _container = new UnityContainer();
        //private IRegionManager _regionManager = new RegionManager();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
        //protected override void OnActivated(EventArgs e)
        //{
        //    ApplicationContext.Container = _container;
        //    _container.RegisterType<RegionManager>();
        //    _container.RegisterType<MainView.NavigationService>();
        //    _container.RegisterType<MainViewModel.MainViewModel>();
        //    _container.RegisterType<MainView.MainView>(Views.MainView);

        //    //_regionManager.
        //    //_container.RegisterType<IRegionManager>().RegisterInstance();

        //    //ServiceLocator.Current.GetInstance<IRegionManager>();
        //    //_regionManager.RegisterViewWithRegion<MainView.MainView, MainViewModel.MainViewModel>(Regions.MainViewRegion)

        //    base.OnActivated(e);
        //}
    }
}
