using Microsoft.EntityFrameworkCore;
using PedidosAppi.Data;
using PedidosAppi.Interfaces;

namespace PedidosAppi.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly PedidosAppiContext _context;

        public UserManagerService (PedidosAppiContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidateUser(string user, string recoveryCode)
        {
            bool userValidate = false;

            var userEntity = await _context.CodigosRecuperacion
                .Where(cr => cr.Usuario == user
                    && cr.CodRecuperacion == recoveryCode
                    && cr.FechaExpiracion >= DateTime.Now)
                .FirstOrDefaultAsync();

            if (userEntity != null && userEntity.CodRecuperacion.Equals(recoveryCode, StringComparison.Ordinal))
            {
                userValidate = true;
            }

            return userValidate;
        }

        public async Task ChangePassword(string user, string password)
        {
            var userEntity = await _context.Usuarios
                .Where(u => u.Usuario == user)
                .FirstOrDefaultAsync();

            if(userEntity is null)
            {
                throw new Exception($"Error: El usuario {user} no existe.");
            }

            try
            {
                userEntity.Clave = BCrypt.Net.BCrypt.HashPassword(password);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}
