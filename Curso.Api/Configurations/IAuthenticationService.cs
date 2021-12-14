using Curso.Api.Models.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curso.Api.Configurations
{
   public interface IAuthenticationService
    {

        string GerarToken(UsuarioViewModelOutput usuarioViewModelOutput);

    }
}
