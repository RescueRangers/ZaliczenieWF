using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Wpf.Core;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Formatting;

namespace ZaliczenieWF.WPF
{
    internal class Setup : MvxWpfSetup<Core.App>
    {
        protected override ILoggerFactory? CreateLogFactory()
        {
#if DEBUG
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

#else
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Error()
                .WriteTo.Console()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
#endif

            return new SerilogLoggerFactory();
        }
        protected override ILoggerProvider? CreateLogProvider()
        {
            return new SerilogLoggerProvider();
        }
    }
}
