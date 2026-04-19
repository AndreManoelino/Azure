using CorporateIdentityManager.Application.Services;
using CorporateIdentityManager.Controllers.Requests;
using CorporateIdentityManager.Controllers.Auth.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CorporateIdentityManager.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly TokenService _tokenService;

        public AuthController(UsuarioService usuarioService, TokenService tokenService)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _usuarioService.ObterPorUpn(request.Upn);

            if (usuario == null)
                return Unauthorized("Usuário não encontrado");
            if (usuario.ContaBloqueada)
                return Unauthorized("Conta bloqueada. Procure o administrador.");
            if (!BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
                return Unauthorized("Senha inválida");

            if (usuario.PrimeiroLogin)
            {
                return Unauthorized(new
                {
                    mensagem = "É necessário alterar a senha"
                });
            }

            var token = _tokenService.GerarToken(usuario);

            var response = new
            {
                nome = usuario.Nome,
                email = usuario.Email,
                upn = usuario.UPN,
                token,
                grupos = usuario.UsuarioGrupos?
                    .Where(ug => ug.Grupo != null)
                    .Select(ug => ug.Grupo!.Nome)
                    .ToList() ?? []
            };

            return Ok(response);
        }
        [HttpPost("alterar-senha")]
        public async Task<IActionResult> AlterarSenha([FromBody]AlterarSenhaRequest request)
        {
            var usuario = await _usuarioService.ObterPorUpn(request.Upn);
            if (usuario == null)
                return NotFound("Usuário não encontrado");
            if (string.IsNullOrEmpty(usuario.SenhaHash) ||
                !BCrypt.Net.BCrypt.Verify(request.SenhaAtual, usuario.SenhaHash))
            {
                return Unauthorized("Senha inválida");
            }
            var novaSenhaHash = BCrypt.Net.BCrypt.HashPassword(request.NovaSenha);
            usuario.DefinirNovaSenha(novaSenhaHash);
            await _usuarioService.Atualizar(usuario);
            return Ok(new
            {
                mensagem = "Senha alterada "
            });
        }

    }
}