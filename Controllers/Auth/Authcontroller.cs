using CorporateIdentityManager.Application.Services;
using CorporateIdentityManager.Controllers.Requests;
using CorporateIdentityManager.Controllers.Auth.Responses;
using CorporateIdentityManager.Persistence.Context;
using CorporateIdentityManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
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

            // O carimbo de ponto virtual! Salva quando logou.
            usuario.AtualizarUltimoLogin();
            await _usuarioService.Atualizar(usuario);

            var token = _tokenService.GerarToken(usuario);

            var response = new
            {
                nome = usuario.Nome,
                email = usuario.Email,
                upn = usuario.UPN,
                token,
                ultimoLogin = usuario.UltimoLogin,
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

        [HttpPost("seed-admin")]
        public async Task<IActionResult> SeedAdmin([FromServices] ActiveDirectoryDbContext context)
        {
            var adminGroup = await context.Grupos.FirstOrDefaultAsync(g => g.Nome == "Admin_Global");
            if (adminGroup == null)
            {
                adminGroup = new Grupo("Admin_Global", "Acesso Total", true, true, false, (CorporateIdentityManager.Domain.Enums.TipoGrupo)0);
                context.Grupos.Add(adminGroup);
                await context.SaveChangesAsync();
            }

            var existingAdmin = await context.Usuarios.Include(u => u.UsuarioGrupos).FirstOrDefaultAsync(p => p.Email == "admin@empresa.com");
            if (existingAdmin != null) 
            {
                if (!existingAdmin.UsuarioGrupos.Any(ug => ug.GrupoId == adminGroup.Id))
                {
                    context.UsuarioGrupos.Add(new UsuarioGrupo(existingAdmin.Id, adminGroup.Id));
                    await context.SaveChangesAsync();
                    return Ok(new { message = "Admin já existia, mas o grupo estava quebrado e foi corrigido! UPN: admin Senha: admin123" });
                }
                return BadRequest("Admin já existe e já possui o grupo corretamente.");
            }

            var organizacao = await context.Organizacoes.FirstOrDefaultAsync();
            if (organizacao == null)
            {
                organizacao = new Organizacao("Sede", "123", "empresa.com", "tenant");
                context.Organizacoes.Add(organizacao);
            }

            var departamento = await context.Departamentos.FirstOrDefaultAsync(d => d.Nome == "TI");
            if (departamento == null)
            {
                departamento = new Departamento("TI", "Departamento TI", organizacao.Id);
                context.Departamentos.Add(departamento);
            }

            var unidade = await context.UnidadesOrganizacionais.FirstOrDefaultAsync(u => u.Nome == "Matriz");
            if (unidade == null)
            {
                unidade = new UnidadeOrganizacional("Matriz", "Matriz", departamento.Id, null);
                context.UnidadesOrganizacionais.Add(unidade);
            }
            await context.SaveChangesAsync();

            var admin = new Usuario("Global", "Admin", "00000000000", "admin@empresa.com", "000", DateTime.Now, "admin", Guid.NewGuid().ToString(), "empresa.com", BCrypt.Net.BCrypt.HashPassword("admin123"), organizacao.Id, departamento.Id, unidade.Id);
            admin.DefinirNovaSenha(BCrypt.Net.BCrypt.HashPassword("admin123")); 

            context.Usuarios.Add(admin);
            await context.SaveChangesAsync();

            var usuarioGrupo = new UsuarioGrupo(admin.Id, adminGroup.Id);
            context.UsuarioGrupos.Add(usuarioGrupo);
            await context.SaveChangesAsync();

            return Ok(new { message = "Admin Global criado. UPN: admin Senha: admin123" });
        }

    }
}