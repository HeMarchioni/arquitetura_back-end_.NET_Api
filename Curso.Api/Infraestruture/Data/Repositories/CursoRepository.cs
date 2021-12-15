using Curso.Api.Business.Entities;
using Curso.Api.Business.Repositories;
using Curso.Api.Infraestruture.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace curso.api.Infraestruture.Data.Repositories
{
    public class CursoRepository : ICursoRepository
    {
        private readonly CursoDbContext _contexto;

        public CursoRepository(CursoDbContext contexto)
        {
            _contexto = contexto;
        }

        public void Adicionar(Cursos curso)
        {
            _contexto.Cursos.Add(curso);
        }

        public void Commit()
        {
            _contexto.SaveChanges();
        }

        public IList<Cursos> ObterPorUsuario(int codigoUsuario)
        {
            return _contexto.Cursos.Include(i => i.Usuario).Where(w => w.CodigoUsuario == codigoUsuario).ToList(); //-> include como se fosse um iner join
        }
    }
}
