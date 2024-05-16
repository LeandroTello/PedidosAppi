using MimeKit;
using PedidosAppi.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace PedidosAppi.Services
{
    public class SendEmailService : ISendEmailService
    {
        public async Task SendEmailRecover(string emailFrom, string emailTo, string emailBody, string emailPass, string codRecuperacion)
        {
            string servidor = "smtp-mail.outlook.com";
            int puerto = 587;

            try
            {
                var emailMensaje = new MimeMessage();

                emailMensaje.From.Add(new MailboxAddress("PedidosApp", emailFrom));

                emailMensaje.To.Add(new MailboxAddress("Usuario", emailTo));

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = $"Su código de recuperación es: {codRecuperacion}<br>Por favor ingresar a este <a href='https://localhost:7006/Access/RecoverUser'>link</a> para recuperar su usuario.";

                emailMensaje.Body = bodyBuilder.ToMessageBody();

                using (var memoryStream = new MemoryStream())
                {

                    using (var smtpClient = new SmtpClient())
                    {
                        smtpClient.CheckCertificateRevocation = false;
                        await smtpClient.ConnectAsync(servidor, puerto, SecureSocketOptions.StartTls);
                        await smtpClient.AuthenticateAsync(emailFrom, emailPass);
                        await smtpClient.SendAsync(emailMensaje);
                        await smtpClient.DisconnectAsync(true);
                    }
                }

            }
            catch (Exception ex) 
            {
                throw new Exception($"Error: {ex.Message}");
            }

            throw new NotImplementedException();
        }
    }
}