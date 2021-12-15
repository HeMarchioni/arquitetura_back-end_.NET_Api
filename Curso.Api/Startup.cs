using curso.api.Infraestruture.Data.Repositories;
using Curso.Api.Business.Repositories;
using Curso.Api.Configurations;
using Curso.Api.Infraestruture.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Curso.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true; //-> desabilitar os retornos padrçoes das solicitações (mas tem que criar o seu proprio)
            });


            services.AddSwaggerGen(c =>   // -> configuração e add swagger
            {

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme          //-> configuração de Segurança no swagger
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });



                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; //-> arquivo xml (para usar no swagger)
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);  //-> local do arquivo
                c.IncludeXmlComments(xmlPath);
            });



            var secret = Encoding.ASCII.GetBytes(Configuration.GetSection("JwtConfigurations:Secret").Value);  // -> chave para o JWT

            services.AddAuthentication(x =>    // -> configuração e add autentication
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            
            .AddJwtBearer(x =>
              {
                  x.RequireHttpsMetadata = false;                       //-> nao ta usando https por hora
                  x.SaveToken = true;                                  //-> salva o token
                  x.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,        
                      IssuerSigningKey = new SymmetricSecurityKey(secret),    //-> chave simetrica
                      ValidateIssuer = false,
                      ValidateAudience = false
                  };
              });


            services.AddDbContext<CursoDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly(typeof(CursoDbContext).Assembly.FullName).EnableRetryOnFailure());
            });


            services.AddScoped<IUsuarioRepository, UsuarioRepository>(); //-> tecnica de injeção de depedencia, sera injetado no controlador sem abrir instancia pq sera na interface (como se fosse instanciado e passado para o controlador)   (Desing I)
            services.AddScoped<ICursoRepository, CursoRepository>();
            services.AddScoped<IAuthenticationService, JwtService>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Curso.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication(); //-> colocar se usar authenticação
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
