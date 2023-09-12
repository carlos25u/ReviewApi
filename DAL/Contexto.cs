using Microsoft.EntityFrameworkCore;
using ReviewApi.Modelos;

namespace ReviewApi.DAL
{
    public class Contexto: DbContext
    {
        public Contexto(DbContextOptions<Contexto> options): base(options) { }

        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Reviews> Reviews { get; set; }


    }
}
