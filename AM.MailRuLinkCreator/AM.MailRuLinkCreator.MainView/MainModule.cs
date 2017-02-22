using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AM.MailRuLinkCreator.MainViewModel;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace AM.MailRuLinkCreator.MainView
{
    public class MainModule : IModule
    {
        private readonly IRegionManager regionManager;
        public MainModule(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

        }
        public void Initialize()
        {
            regionManager.RegisterViewWithRegion(Regions.MainViewRegion, typeof(MainView));
        }
    }
}
