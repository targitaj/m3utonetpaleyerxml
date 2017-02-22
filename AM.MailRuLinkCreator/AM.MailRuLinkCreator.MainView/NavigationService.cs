using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.MailRuLinkCreator.MainViewModel;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace AM.MailRuLinkCreator.MainView
{
    public class NavigationService
    {
        public IRegionManager _regionManager;

        public NavigationService(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void NavigateToRegion(string regionName, string viewName)
        {
            _regionManager.RequestNavigate(Regions.MainViewRegion, new Uri(viewName, UriKind.Relative));
        }
    }
}
