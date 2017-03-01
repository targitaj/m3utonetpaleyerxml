using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AM.MailRuLinkCreator.MainViewModel;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace AM.MailRuLinkCreator.MainViewModel
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
            var rm = _regionManager;
            var region = (Region)rm.Regions[Regions.MainViewRegion];
            var views = region.Views.ToList();

            foreach (var view in views)
            {
                region.Deactivate(view);

                var lifeTime = view.GetType().GetProperty("DataContext")?.GetValue(view) as IRegionMemberLifetime;

                if (lifeTime?.KeepAlive == true)
                {
                    region.Remove(view);
                }
            }

            rm.RequestNavigate(Regions.MainViewRegion, new Uri("/" + viewName, UriKind.Relative), res =>
            {
                
            });
        }
    }
}
