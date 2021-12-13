using Curso.Api.Infraestruture.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Curso.Api.Configurations
{
    public class DbFactoryDbContext : IDesignTimeDbContextFactory<CursoDbContext>  //-> Classe para configuração da conexão com o banco (muitas vezes colocava no Startup)
    {
        public CursoDbContext CreateDbContext(string[] args)
        {


            var configuration = new ConfigurationBuilder()           //-> para usar a string de conexão que esta no appsettings
                                    .AddJsonFile("appsettings.json")
                                    .Build();


            var optionsBuilder = new DbContextOptionsBuilder<CursoDbContext>();    
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            CursoDbContext contexto = new CursoDbContext(optionsBuilder.Options);
            return contexto;



        }
    }
}
