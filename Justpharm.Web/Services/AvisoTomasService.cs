using Justpharm.Web.Hubs;
using Justpharm.Web.Models;
using Justpharm.Web.Repository;
using Justpharm.Web.Services.Email;
using log4net;
using Microsoft.AspNetCore.SignalR;

namespace Justpharm.Web.Services
{
    public class AvisoTomasService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly EmailSender _emailService;
        private readonly DbQry Qry;
        private readonly ILog _log;

        public AvisoTomasService(IServiceProvider services, EmailSender emailService, DbQry db, ILog log)
        {
            _serviceProvider = services;
            _emailService = emailService;
            Qry = db;
            _log = log;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _log.Info("Se ha iniciado el servicio de aviso de tomas.");
                // Obtener la lista de "Tomas" con StartTime coincidente
                List<Toma>? tomas = Qry.All<Toma>(t => t.StartTime.Date == DateTime.Now.Date && (t.Ingerido == null || t.Ingerido == false)).ToList();

                if (tomas == null) continue;

                foreach (var toma in tomas)
                {
                    if (toma.StartTime.TimeOfDay > DateTime.Now.TimeOfDay) continue;

                    string to = toma.EmailAviso;
                    string subject = $"Recordatorio de toma {toma.Titulo}";
                    string body = $@"
                                <html>
                                <body>
                                    <div style='font-family: Arial, sans-serif; padding: 20px; background-color: #f9f9f9; border: 1px solid #ddd; border-radius: 10px;'>
                                        <h2 style='color: #333;'>Recordatorio de toma</h2>
                                        <p>Hola,</p>
                                        <p>Este es un recordatorio para que tome su medicamento <strong>{toma.Titulo}</strong> a las <strong>{toma.StartTime.ToShortTimeString()}</strong>.</p>
                                        <p>Es muy importante que siga las instrucciones de su médico para asegurarse de que el tratamiento sea efectivo.</p>
                                        <p style='margin-top: 20px;'>Saludos,</p>
                                        <p>Justpharm</p>
                                    </div>
                                </body>
                                </html>";

                    _log.Info($"Enviando correo a {to} con el asunto {subject}");
                    await _emailService.EmailSend(to!, subject, body);
                }

                // Verificar cada 5 minutos (300,000 milisegundos)
                await Task.Delay(300000, stoppingToken);
            }
        }


        private async Task EnviarNotificacion(Toma toma)
        {
            IHubContext<NotificationHub>? _notifHub = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();
            await _notifHub.Clients.All.SendAsync("ReceiveNotification", "Aviso de toma", $"Recuerde tomar su toma del tratamiento {toma.Titulo}");
        }
    }
}
