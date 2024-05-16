namespace PedidosAppi.Interfaces
{
    public interface ISendEmailService
    {
        Task SendEmailRecover(string emailFrom, string emailTo, string emailBody, string emailPass, string codRecuperacion);
    }
}
