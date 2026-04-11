using CorporateIdentityManager.Domain.Entities;
using CorporateIdentityManager.Persistence.Context;
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
        public async Task AplicarPoliticasDaOU(Guid usuarioId, Guid unidadeOrganizacionalId)
        {
            await _grupoService.AtribuirGruposPorUnidadeOrganizacional(usuarioId, unidadeOrganizacionalId);
        }
    }
}