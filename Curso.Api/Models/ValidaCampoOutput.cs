using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Curso.Api.Models
{
    public class ValidaCampoOutput
    {


        public IEnumerable<string> Erros { get; private set; }

        public ValidaCampoOutput(IEnumerable<string> erros)
        {
            Erros = erros;
        }





    }
}
