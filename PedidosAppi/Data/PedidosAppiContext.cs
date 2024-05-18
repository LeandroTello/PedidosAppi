using Microsoft.EntityFrameworkCore;

namespace PedidosAppi.Data
{
    public class PedidosAppiContext : DbContext
    {
        public PedidosAppiContext(DbContextOptions<PedidosAppiContext> options) : base(options) { }

        public DbSet<PedidosAppi.Models.CodigoRecuperacionModel> CodigosRecuperacion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
