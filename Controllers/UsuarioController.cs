using CorporateIdentityManager.Application.Services;
using CorporateIdentityManager.Controllers.Requests;
using CorporateIdentityManager.Domain.Entities;
using CorporateIdentityManager.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
namespace CorporateIdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly OrganizacaoService _organizacaoService;
        private readonly ActiveDirectoryDbContext _context;
        public UsuarioController(UsuarioService usuarioService, OrganizacaoService organizacaoService, ActiveDirectoryDbContext context)
        {
            _usuarioService = usuarioService;
            _organizacaoService = organizacaoService;
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioRequest request)
        {
            var usuario = new Usuario(
                request.Nome,
                request.Sobrenome,
                request.CPF,
                request.Email,
                request.Telefone,
                request.DataNascimento,
                request.Email,
                Guid.NewGuid().ToString(),
                "empresa.com",
                BCrypt.Net.BCrypt.HashPassword("123456"),
                request.OrganizacaoId,
                request.DepartamentoId,
                request.UnidadeOrganizacionalId
            );

            var usuarioId = await _usuarioService.CriarUsuario(
                usuario,
                request.UnidadeOrganizacionalId
            );

            return Ok(new { usuarioId });
        }
        [HttpGet]
        public async Task<IActionResult> ObterUsuario(Guid id)
        {
            var usuario = await _usuarioService.ObterPorId(id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }

    }
}