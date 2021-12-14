using Curso.Api.Business.Entities;
using Curso.Api.Business.Repositories;
using Curso.Api.Infraestruture.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Infraestruture.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository  //-> desing patter (para nao acessar direto o DbContext pelo controller)
    {
        private readonly CursoDbContext _contexto;

        public UsuarioRepository(CursoDbContext contexto)
        {
            _contexto = contexto;
        }

        public void Adicionar(Usuario usuario)
        {
            _contexto.Usuario.Add(usuario);
        }

        public void Commit()
        {
            _contexto.SaveChanges();
        }

        public async Task<Usuario> ObterUsuarioAsync(string login)
        {
            return await _contexto.Usuario.FirstOrDefaultAsync(u => u.Login == login);
        }
    }
}
