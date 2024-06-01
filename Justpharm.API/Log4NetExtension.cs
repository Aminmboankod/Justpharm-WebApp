using log4net;
using log4net.Config;

namespace Justpharm.API;

public static class Log4NetExtension
{
    public static void AddLog4Net(this IServiceCollection services, string log4NetConfigFile = "log4net.config")
    {
        log4NetConfigFile = Path.Combine(AppContext.BaseDirectory, log4NetConfigFile);
        XmlConfigurator.Configure(new FileInfo(log4NetConfigFile));
        services.AddSingleton(LogManager.GetLogger(typeof(Program)));
        services.AddLogging();
    }
}