using MimeKit;
using PedidosAppi.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Security.Cryptography;
using PedidosAppi.Data;
using PedidosAppi.Models;
using Microsoft.EntityFrameworkCore;

namespace PedidosAppi.Services
{
    public class SendEmailService : ISendEmailService
    {
        private readonly PedidosAppiContext _context;

        public SendEmailService(PedidosAppiContext context)
        {
            _context = context;
        }

        public string GenerateRecoveryCode()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[3];
                rng.GetBytes(tokenData);

                return BitConverter.ToString(tokenData).Replace("-", "");
            }
        }

        public async Task SendEmailRecover(string emailFrom, string emailPass, string emailTo, string usuario)
        {
            string servidor = "smtp-mail.outlook.com";
            int puerto = 587;

            string codRecover = GenerateRecoveryCode();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var bodyBuilder = new BodyBuilder();

                    bodyBuilder.HtmlBody = $"Su código de recuperación es: {codRecover}<br>" +
                        $"Por favor ingresar a este <a href='https://localhost:7006/Access/RecoverUser'>link</a> para recuperar su usuario.";

                    var codRecoverExisting = await _context.CodigosRecuperacion
                        .Where(c => c.Usuario == usuario)
                        .FirstOrDefaultAsync();

                    if (codRecoverExisting != null)
                    {
                        bodyBuilder.HtmlBody = $"Su nuevo código de recuperación es: {codRecover}<br>" +
                            $"Por favor, ingrese a este <a href='https://localhost:7006/Access/RecoverUser'>enlace</a> para recuperar su usuario.<br>" +
                            $"Nota: Su código de recuperación anterior ({codRecoverExisting.CodRecuperacion}) ha sido invalidado y ya no se puede utilizar. Asegúrese de usar el nuevo código proporcionado arriba.";
                        _context.Remove(codRecoverExisting);
                    }

                    var codRecuperacionEntity = new CodigoRecuperacionModel
                    {
                        Usuario = usuario,
                        CodRecuperacion = codRecover,
                        FechaExpiracion = DateTime.Now.AddMinutes(5)
                    };
                    _context.Add(codRecuperacionEntity);
                    await _context.SaveChangesAsync();

                    var emailMensaje = new MimeMessage();

                    emailMensaje.From.Add(new MailboxAddress("PedidosApp", emailFrom));

                    emailMensaje.To.Add(new MailboxAddress("Usuario", emailTo));

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

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"Error: {ex.Message}");
                }
            }
        }
    }
}