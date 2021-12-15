using Curso.Api.Business.Entities;
using Curso.Api.Business.Repositories;
using Curso.Api.Models.Curso;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Curso.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]   //-> Solicitando autorização (protege a API) 
    public class CursoController : ControllerBase
    {


        private readonly ICursoRepository _cursoRepository;
        private readonly ILogger<UsuarioController> _logger;

        public CursoController(ICursoRepository cursoRepository, ILogger<UsuarioController> logger)
        {
            _cursoRepository = cursoRepository;
            _logger = logger;
        }






        /// <summary>
        /// Este serviço permite cadastrar curso para o usuário autenticado.
        /// </summary>
        /// <returns>Retorna status 201 e dados do curso do usuário</returns>
        [SwaggerResponse(statusCode: 201, description: "Sucesso ao Cadastrar um curso", Type = typeof(CursoViewModelOutput))]
        [SwaggerResponse(statusCode: 401, description: "Não autorizado")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post(CursoViewModelInput cursoViewModelInput)
        {

            try
            {
                Cursos curso = new Cursos
                {
                    Nome = cursoViewModelInput.Nome,
                    Descricao = cursoViewModelInput.Descricao
                };


                var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);  //-> atravez do token pega quem é o usuario para salvar o curso com id dele
                curso.CodigoUsuario = codigoUsuario;
                _cursoRepository.Adicionar(curso);
                _cursoRepository.Commit();

                var cursoViewModelOutput = new CursoViewModelOutput
                {
                    Nome = curso.Nome,
                    Descricao = curso.Descricao,
                };

                return Created("", cursoViewModelOutput);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }

        }


        /// <summary>
        /// Este serviço permite obter todos os cursos ativos do usuário.
        /// </summary>
        /// <returns>Retorna status ok e dados do curso do usuário</returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao obter os cursos", Type = typeof(CursoViewModelOutput))]
        [SwaggerResponse(statusCode: 401, description: "Não autorizado")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {


            try
            {
                var codigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                var cursos = _cursoRepository.ObterPorUsuario(codigoUsuario)
                    .Select(s => new CursoViewModelOutput()
                    {
                        Nome = s.Nome,
                        Descricao = s.Descricao
                    });

                return Ok(cursos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }


        }

    }
}
