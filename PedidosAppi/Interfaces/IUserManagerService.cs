namespace PedidosAppi.Interfaces
{
    public interface IUserManagerService
    {
        Task<bool> ValidateUser(string user, string recoveryCode);

        Task ChangePassword(string user, string password);
    }
}
