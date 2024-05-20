namespace PedidosAppi.Interfaces
{
    public interface ISendEmailService
    {
        Task SendEmailRecover(string emailFrom, string emailPass, string emailTo, string usuario);

        string GenerateRecoveryCode();

        Task<bool> ValidateUser(string user, string recoveryCode);
    }
}
