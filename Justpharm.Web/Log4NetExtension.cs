using log4net;
using log4net.Config;

namespace Justpharm.Web;

public static class Log4NetExtension
{
    public static void AddLog4Net(this IServiceCollection services, string log4NetConfigFile = "log4net.config")
    {
        var log4netConfigFile = Path.Combine(System.AppContext.BaseDirectory, log4NetConfigFile);
        XmlConfigurator.Configure(new FileInfo(log4netConfigFile));
        services.AddSingleton(LogManager.GetLogger(typeof(Program)));
        services.AddLogging();
    }
}