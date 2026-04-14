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

        public AuthController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _usuarioService.ObterPorUpn(request.Upn);

            if (usuario == null)
                return Unauthorized("Usuário não encontrado");

            if (!BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
                return Unauthorized("Senha inválida");

            if (usuario.PrimeiroLogin)
            {
                return Unauthorized(new
                {
                    mensagem = "É necessário alterar a senha !"
                });
            }

            var response = new LoginResponse
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                UPN = usuario.UPN,
                Grupos = usuario.UsuarioGrupos
                    .Select(ug => ug.Grupo.Nome)
                    .ToList()
            };

            return Ok(response);
        }
        [HttpPost("alterar-senha")]
        public async Task<IActionResult> AlterarSenha([FromBody]AlterarSenhaRequest request)
        {
            var usuario = await _usuarioService.ObterPorUpn(request.Upn);
            if (usuario == null)
                return NotFound("Usuário não encontrado");
            if (usuario.SenhaHash != request.SenhaAtual)
                return Unauthorized("Senha atual inválida");
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