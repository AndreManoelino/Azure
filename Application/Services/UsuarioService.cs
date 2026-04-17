using CorporateIdentityManager.Domain.Entities;
using CorporateIdentityManager.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CorporateIdentityManager.Application.Services
{
    public class UsuarioService
    {
        private readonly ActiveDirectoryDbContext _context;
        private readonly GrupoService _grupoService;

        public UsuarioService(
            ActiveDirectoryDbContext context,
            GrupoService grupoService)
        {
            _context = context;
            _grupoService = grupoService;
        }

        public async Task<Guid> CriarUsuario(Pessoa usuario, Guid unidadeOrganizacionalId)
        {
            _context.Pessoas.Add(usuario);
            await _context.SaveChangesAsync();

            await _grupoService.AtribuirGruposPorUnidadeOrganizacional(usuario.Id, unidadeOrganizacionalId);

            return usuario.Id;
        }
        public Task AplicarPoliticasDaOU(Guid usuarioId, Guid unidadeOrganizacionalId)
        {
            return _grupoService.AtribuirGruposPorUnidadeOrganizacional(usuarioId, unidadeOrganizacionalId);
        }

        public async Task<Guid> CriarUsuario(
            string nome,
            string sobrenome,
            string email,
            string cpf,
            string telefone,
            DateTime dataNascimento,
            Guid organizacaoId,
            Guid departamentoId,
            Guid unidadeOrganizacionalId)
        {
            var usuario = new Usuario(
                nome,
                sobrenome,
                cpf,
                email,
                telefone,
                dataNascimento,
                email,
                Guid.NewGuid().ToString(),
                "empresa.local",
                BCrypt.Net.BCrypt.HashPassword("hash123"),
                organizacaoId,
                departamentoId,
                unidadeOrganizacionalId
            );

            _context.Pessoas.Add(usuario);

            await _context.SaveChangesAsync();

            await _grupoService.AtribuirGruposPorUnidadeOrganizacional(
                usuario.Id,
                unidadeOrganizacionalId
            );

            return usuario.Id;
        }

        public Task<Usuario?> ObterPorId(Guid id)
        {
            return _context.Set<Usuario>().FindAsync(id).AsTask();
        }
        public async Task<Usuario?> ObterPorUpn(string upn)
        {
            return await _context.Set<Usuario>()
                .Include(u => u.UsuarioGrupos)
                .ThenInclude(ug => ug.Grupo)
                .FirstOrDefaultAsync(u => u.UPN == upn);
        }
        public async Task Atualizar(Usuario usuario)
        {
            var existente = await _context.Set<Usuario>()
                .FirstAsync(u => u.Id == usuario.Id);

            if (existente == null) return;

            existente.DefinirNovaSenha(usuario.SenhaHash);
            await _context.SaveChangesAsync();
        }
    }
}