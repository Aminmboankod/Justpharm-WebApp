using Justpharm.Web.Models;
using Justpharm.Web.Repository;
using Microsoft.AspNetCore.SignalR;

namespace Justpharm.Web.Hubs
{

    public class NotificationHelper<THub> where THub : Hub
    {
        public readonly IHubContext<THub> Hub;
        private readonly DbQry DbQry;
        public NotificationHelper(IHubContext<THub> hub, DbQry qry)
        {
            this.Hub = hub;
            DbQry = qry;
        }

        public void GuardarNotificacion(Notificacion noti)
        {
        }

        public async Task BroadcastNotification(Notificacion noti, bool onlineOnly = false)
        {
            // tipoNotificacion = 0 siempre (los int no son NULL)

            DbQry.Insert(noti); // guardar la notificacion

            //_logger.LogDebug($"Nueva notificacion para todos: {JsonSerializer.Serialize(noti)}. Online: {onlineOnly}. ConnectionIds: {JsonSerializer.Serialize(Startup.ConnectionIds)}");
            if (!onlineOnly)
                GuardarNotificacion(noti);
            else
                GuardarNotificacion(noti);
            //await Hub.Clients.All.SendAsync("ReceiveNotification", noti); // enviarle la notificacion a los conectados
        }

        public async Task GroupNotification(string group, Notificacion noti, bool onlineOnly = false)
        {
            // tipoNotificacion = 0 siempre

            DbQry.Insert(noti);

            //_logger.LogDebug($"Nueva notificacion para el grupo {group}: {JsonSerializer.Serialize(noti)}. OnlineOnly: {onlineOnly}. ConnectionIds: {JsonSerializer.Serialize(Startup.ConnectionIds)}");
            if (!onlineOnly)
                GuardarNotificacion(noti);
            else
                GuardarNotificacion(noti);
            // notificamos al grupo
            //await Hub.Clients.Group(group).SendAsync("ReceiveNotification", noti);
        }

        public async Task PersonalNotification(Notificacion noti, string userId)
        {

            DbQry.Insert(noti);

            //_logger.LogDebug($"Nueva notificacion para el usuario {userId}: {JsonSerializer.Serialize(noti)}");
            GuardarNotificacion(noti);
            //await Hub.Clients.User(userId).SendAsync("ReceiveNotification", noti);
        }
    }
}
