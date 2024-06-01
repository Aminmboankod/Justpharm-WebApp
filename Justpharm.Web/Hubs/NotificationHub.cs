using Justpharm.Web.Models;
using log4net;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Justpharm.Web.Hubs
{
    public class NotificationHub : Hub
    {
        public static readonly string NotificationUri = "/notifications";

        private readonly NotificationHelper<NotificationHub> _helper;
        private readonly ILog _logger;
        public NotificationHub(NotificationHelper<NotificationHub> helper, ILog logger)
        {
            _helper = helper;
            _logger = logger;
        }

        public async Task BroadcastNotification(Notificacion noti) =>
            await _helper.BroadcastNotification(noti);

        public async Task GroupNotification(string group, Notificacion noti) =>
            await _helper.GroupNotification(group, noti);

        public async Task PersonalNotification(Notificacion noti, string userId) =>
            await _helper.PersonalNotification(noti, userId);

        public async Task OnlineNotification(Notificacion noti) =>
            await _helper.BroadcastNotification(noti, true);

        public async Task OnlineGroupNotification(string group, Notificacion noti) =>
            await _helper.GroupNotification(group, noti, true);

        public override Task OnConnectedAsync()
        {
            var user = Context.User;

            //_logger.Debug("Se ha conectado un usuario (sin identificar)");
            if (user == null || (bool)user.Identity?.IsAuthenticated == false) return base.OnConnectedAsync();
            var roles = user.FindAll(claim => claim.Type == ClaimTypes.Role).ToList();
            // añadirlo al grupo
            roles.ForEach(claim => Groups.AddToGroupAsync(Context.ConnectionId, claim.Value));

            var nameIdentifier = user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier, null)?.Value;
            if (!string.IsNullOrEmpty(nameIdentifier))
            {
                //_logger.Debug($"Se ha conectado el usuario {nameIdentifier} al hub de notificaciones, con los roles {string.Join(", ", roles.Select(x => x.Value))}");
            } // añadir una vez

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var user = Context.User;
            //_logger.LogDebug($"Se ha desconectado un usuario (sin identificar)");
            if (user == null || (bool)user.Identity?.IsAuthenticated) return base.OnDisconnectedAsync(exception);

            var nameIdentifier = user.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier, null)?.Value;
            if (!string.IsNullOrEmpty(nameIdentifier))
            {
                //Startup.ConnectionIds.Remove(nameIdentifier);
                //_logger.LogDebug("Se ha desconectado el usuario {} del hub de notificaciones", nameIdentifier);
                //if (exception != null)
                //    _logger.LogDebug("{}", exception.Message);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
