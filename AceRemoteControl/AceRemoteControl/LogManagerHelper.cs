using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace AceRemoteControl
{
    public static class LogManagerHelper
    {
        static LogManagerHelper()
        {
            var hierarchy = (Hierarchy) LogManager.GetRepository();

            var patternLayout = new PatternLayout { ConversionPattern = "%d %-5p %c - %m%n" };
            patternLayout.ActivateOptions();

            var roller = new RollingFileAppender
            {
                AppendToFile = true,
                File = @"Logs\log.txt",
                Layout = patternLayout,
                MaxSizeRollBackups = 10,
                MaximumFileSize = "5MB",
                RollingStyle = RollingFileAppender.RollingMode.Size,
                StaticLogFileName = true
            };

            roller.AddFilter(new LevelRangeFilter()
            {
                LevelMin = Level.Debug
            });

            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            MemoryAppender memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.Debug;
            hierarchy.Configured = true;
        }

        public static ILog GetLogger<TLgger>()
        {
            return LogManager.GetLogger(typeof(TLgger));
        }
    }
}
