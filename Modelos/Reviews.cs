using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewApi.Modelos
{
    public class Reviews
    {
        [Key]
        public int ReviewId { get; set; }
        public String? Restaurante { get; set; }
        public String? Comentario { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int UsuariosId { get; set; }

        [ForeignKey("UsuariosId")]
        public Usuarios usuarios { get; set; }


    }
}
