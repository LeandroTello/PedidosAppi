namespace PedidosAppi.Interfaces
{
    public interface ISendEmailService
    {
        Task SendEmailRecover(string emailFrom, string emailTo, string emailPass, string codRecuperacion);
    }
}
