using System.ComponentModel.DataAnnotations;

namespace ReviewApi.Modelos
{
    public class Usuarios
    {
        [Key] 
        public int UsuariosId { get; set; }
        public String? Username { get; set; }
        public String? Password { get; set; }
        public String? ConfirmPassword { get; set; }
    }
}
