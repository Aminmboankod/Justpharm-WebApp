namespace Justpharm.Web.Services.Notification
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNotifications(this IServiceCollection services)
        {
            return services.AddScoped<INotificationService, NotificationService>();
        }
    }
}
