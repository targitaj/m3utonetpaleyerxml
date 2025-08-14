using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeDownloader.Services
{
    public static class ServiceLocator
    {
        public static IServiceProvider Provider { get; set; } = default!;
        public static T Resolve<T>() where T : notnull => Provider.GetRequiredService<T>();
    }
}
