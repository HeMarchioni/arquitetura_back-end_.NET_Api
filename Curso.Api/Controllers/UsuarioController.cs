using Curso.Api.Business.Entities;
using Curso.Api.Business.Repositories;
using Curso.Api.Configurations;
using Curso.Api.Filters;
using Curso.Api.Infraestruture.Data;
using Curso.Api.Models;
using Curso.Api.Models.Usuarios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Curso.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {



        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IAuthenticationService _authenticationService;


        public UsuarioController(ILogger<UsuarioController> logger,IUsuarioRepository usuarioRepository,IAuthenticationService authenticationService)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
            _authenticationService = authenticationService;
        }




        /// <summary>
        /// Este serviço permite autenticar um usuário cadastrado e ativo.
        /// </summary>
        /// <param name="loginInput">View model do login</param>
        /// <returns>Retorna status ok, dados do usuario e o token em caso de sucesso</returns>
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao autenticar", Type = typeof(LoginInput))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios", Type = typeof(ValidaCampoOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErroGenerico))]
        [HttpPost]
        [Route("logar")]
        [ValidacaoModelStateCustomizado]  // -> se pegar algum erro na validação
        public async Task<IActionResult> Logar([FromBody] LoginInput loginInput)
        {

            try
            {
                var usuario = await _usuarioRepository.ObterUsuarioAsync(loginInput.Login);

                if (usuario == null)
                {
                    return BadRequest("Houve um erro ao tentar acessar.");
                }

                //if (usuario.Senha != loginViewModel.Senha.GerarSenhaCriptografada())
                //{
                //    return BadRequest("Houve um erro ao tentar acessar.");
                //}

                var usuarioViewModelOutput = new UsuarioViewModelOutput()
                {
                    Codigo = usuario.Codigo,
                    Login = loginInput.Login,
                    Email = usuario.Email
                };

                var token = _authenticationService.GerarToken(usuarioViewModelOutput);

                return Ok(new LoginViewModelOutput
                {
                    Token = token,
                    Usuario = usuarioViewModelOutput
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }



        }



        /// <summary>
        /// Este serviço permite cadastrar um usuário cadastrado não existente.
        /// </summary>
        /// <param name="loginInput">View model do registro de login</param>
        [SwaggerResponse(statusCode: 201, description: "Sucesso ao cadastrar", Type = typeof(RegistraInput))]
        [SwaggerResponse(statusCode: 400, description: "Campos obrigatórios", Type = typeof(ValidaCampoOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro interno", Type = typeof(ErroGenerico))]
        [HttpPost]
        [Route("registrar")]
        [ValidacaoModelStateCustomizado]
        public async Task<IActionResult> Registrar([FromBody] RegistrarInput registrarInput)
        {

            try
            {
                var usuario = await _usuarioRepository.ObterUsuarioAsync(registrarInput.Login);

                if (usuario != null)
                {
                    return BadRequest("Usuário já cadastrado");
                }

                usuario = new Usuario
                {
                    Login = registrarInput.Login,
                    Senha = registrarInput.Senha,
                    Email = registrarInput.Email
                };
                _usuarioRepository.Adicionar(usuario);
                _usuarioRepository.Commit();

                return Created("", registrarInput);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new StatusCodeResult(500);
            }

        }

    }
}
