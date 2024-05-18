namespace PedidosAppi.Interfaces
{
    public interface ISendEmailService
    {
        Task SendEmailRecover(string emailFrom, string emailPass, string emailTo, string usuario);

        string GenerateRecoveryCode();
    }
}
