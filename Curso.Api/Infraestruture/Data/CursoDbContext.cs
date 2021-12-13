using Curso.Api.Business.Entities;
using Curso.Api.Infraestruture.Data.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Curso.Api.Infraestruture.Data
{
    public class CursoDbContext : DbContext
    {
        public CursoDbContext(DbContextOptions<CursoDbContext> options) :base(options)
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CursoMapping());    //-Passa a configuração de relacionamento feita no cursoMapping (IEntityTypeConfiguration)
            modelBuilder.ApplyConfiguration(new UsuarioMapping());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Usuario> Usuario { get; set; }   //-> sempre referenciar as classes que serao tabelas
        public DbSet<Cursos> Cursos { get; set; }
    }
}
