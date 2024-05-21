using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PedidosAppi.Models
{
    [Table("Usuarios")]
    public class UsuarioModel
    {
        [Key]
        public string Usuario { get; set; }

        public string Clave { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Dni { get; set; }

        public string Email { get; set; }

        public string Telefono { get; set; }

        public int Id_Direccion { get; set; }

        public string Cod_Cliente { get; set; }

        public int Id_Rol { get; set; }

    }
}
