using CorporateIdentityManager.Domain.Entities;
using CorporateIdentityManager.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CorporateIdentityManager.Application.Services
{
    public class UsuarioService(ActiveDirectoryDbContext context, GrupoService grupoService)
    {
        private readonly ActiveDirectoryDbContext _context = context;
        private readonly GrupoService _grupoService = grupoService;


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
        public Task<Usuario?> ObterPorUpn(string upn)
        {
            return _context.Set<Usuario>()
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

        public async Task<IEnumerable<Usuario>> BuscarPorNome(string nome)
        {
            return await _context.Set<Usuario>()
                .Where(u => u.Nome.Contains(nome) || u.Sobrenome.Contains(nome))
                .ToListAsync();
        }

        public async Task BloquearUsuario(Guid id)
        {
            var usuario = await _context.Set<Usuario>().FindAsync(id);
            if (usuario == null) return;

            usuario.BloquearConta();

            // Remove as licenças financeiras para evitar gastos
            var licencas = _context.UsuarioLicencas.Where(l => l.UsuarioId == id);
            _context.UsuarioLicencas.RemoveRange(licencas);

            // Nota: Os grupos foram mantidos, já que atuam como "cargos" e servem de histórico

            await _context.SaveChangesAsync();
        }
    }
}