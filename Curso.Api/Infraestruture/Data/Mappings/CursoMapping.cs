using Curso.Api.Business.Entities;
using Microsoft.EntityFrameworkCore;    //-> usou EntitiFramework + EntitiFramework.relational
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Curso.Api.Infraestruture.Data.Mappings
{
    public class CursoMapping : IEntityTypeConfiguration<Cursos>
    {
        public void Configure(EntityTypeBuilder<Cursos> builder)  // -> colocando a configuração da entitade poderia ser feito na propria classe
        {
            builder.ToTable("TB_CURSO");
            builder.HasKey(p => p.Codigo);                  //-> PK
            builder.Property(p => p.Codigo).ValueGeneratedOnAdd();    //-> gerar o valo solo 
            builder.Property(p => p.Nome);
            builder.Property(p => p.Descricao);
            builder.HasOne(p => p.Usuario)              // -> relacionamentos
                .WithMany().HasForeignKey(fk => fk.CodigoUsuario);
        }

       
    }
}
