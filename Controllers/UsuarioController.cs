using CorporateIdentityManager.Application.Services;
using CorporateIdentityManager.Controllers.Requests;
using CorporateIdentityManager.Domain.Entities;
using CorporateIdentityManager.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CorporateIdentityManager.Application.Helpers;

namespace CorporateIdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly ActiveDirectoryDbContext _context;

        public UsuarioController(
            UsuarioService usuarioService,
            ActiveDirectoryDbContext context)
        {
            _usuarioService = usuarioService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario([FromBody] CriarUsuarioRequest request)
        {
            var cpfValido = CpfHelper.ApenasNumeros(request.CPF);
            if(!CpfHelper.EhValido(request.CPF))
                return BadRequest("CPF inválido");

            var cpfExistente = await _context.Pessoas
                .FirstOrDefaultAsync(p => p.Cpf == cpfValido);
            if(cpfExistente != null)

                return BadRequest("Já existe um usuário com esse CPF");
            var usuarioExistente = await _context.Pessoas
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuarioExistente != null)
                return BadRequest("Já existe um usuário com esse e-mail");

            var organizacao = await _context.Organizacoes
                .FirstOrDefaultAsync(o => o.Nome == request.OrganizacaoNome);

            var departamento = await _context.Departamentos
                .FirstOrDefaultAsync(d => d.Nome == request.DepartamentoNome);

            var unidade = await _context.UnidadesOrganizacionais
                .FirstOrDefaultAsync(u => u.Nome == request.UnidadeNome);

            if (organizacao == null || departamento == null || unidade == null)
                return BadRequest("Organização, Departamento ou Unidade inválidos");

            var usuario = new Usuario(
                request.Nome,
                request.Sobrenome,
                cpfValido,
                request.Email,
                request.Telefone,
                request.DataNascimento,
                request.Email,
                Guid.NewGuid().ToString(),
                "empresa.com",
                BCrypt.Net.BCrypt.HashPassword("123456"),
                organizacao.Id,
                departamento.Id,
                unidade.Id
            );

            var usuarioId = await _usuarioService.CriarUsuario(usuario, unidade.Id);

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