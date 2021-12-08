using System.Collections.Generic;
using Curso.Api.Business.Entities;


namespace Curso.Api.Business.Repositories
{
    public interface ICursoRepository
    {
        void Adicionar(Cursos curso);
        void Commit();

        IList<Cursos> ObterPorUsuario(int codigoUsuario);
    }
}
