using Justpharm.Web.Data;
using log4net;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Justpharm.Web.Services.Email
{

    public class EmailSender : IEmailSender<ApplicationUser>
    {
        private readonly ILog _log;
        private readonly IConfiguration _configuration;

        public EmailSender(ILog log, IConfiguration configuration)
        {
            _configuration = configuration;
            _log = log;
        }

        private SmtpClient GetSmtpClient()
        {
            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = _configuration.GetValue("Smtp:Port", 587),
                Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = _configuration.GetValue("Smtp:EnableSsl", true)
            };
            return smtpClient;
        }

        public async Task EmailSend(string to, string subject, string body)
        {
            SmtpClient? smtpClient = GetSmtpClient();
            MailMessage? mailMessage = new MailMessage()
            {
                From = new MailAddress(_configuration["Smtp:Username"]!),
                To = { to },
                Subject = subject,
                Body = body,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                _log.Debug($"Se ha enviado un correo a {to} con el asunto {subject}. Longitud del cuerpo {body.Length}");
            }
            catch (Exception e)
            {
                _log.Error($"No se ha podido envíar un correo.", e);
                throw;
            }
        }

        public async Task Email2FA(string to, string code)
        {
            var body = $@"<div style=""margin: 0 auto; max-width: 400px; border-radius: 15px; box-shadow: 0px 11px 68px 0px rgba(0,0,0,0.38);"">
                                  <div style=""background-color: skyblue; border-radius: 15px 15px 0 0; text-align: center; font-size: 24px; padding: 10px 0; font-family: Arial;"">
                                    Tu código de verificación
                                  </div>
                                  <div style=""background-color: white; border-radius: 0 0 15px 15px; text-align: center; padding: 20px; font-size: 16px; font-family: Arial;"">
                                    <div>
                                      Introduce el siguiente código:
                                    </div>
                                    <div style=""margin-top: 10px; padding: 10px 20px; background-color: gray; color: white; font-family: monospace; letter-spacing: 4px;"">
                                      {code}
                                    </div>
                                  </div>
                                </div>";
            await EmailSend(to, "Código de verificación 2FA", body);
        }

        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            await EmailSend(email, "Confirme su cuenta", $"Confirme su cuenta <a href='{confirmationLink}'>haciendo click aquí</a>.");
        }

        public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            await EmailSend(email, "Recuperación de contraseña", $"Recupere su contraseña <a href='{resetLink}'>haciendo click aquí</a>.");
        }

        public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
            await EmailSend(email, "Cambio de contraseña", $"Recupere su contraseña introduciendo el siguiente código: {resetCode}");
        }
    }

    public static class EmailSenderExtensions
    {
        public static IServiceCollection AddEmailSender(this IServiceCollection services)
        {
            services.AddSingleton<EmailSender>();
            return services;
        }
    }
}
