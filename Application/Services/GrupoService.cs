using CorporateIdentityManager.Domain.Entities;
using CorporateIdentityManager.Persistence.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CorporateIdentityManager.Application.Services
{
    public class GrupoService
    {
        private readonly ActiveDirectoryDbContext _context;
        private readonly LicenciamentoService _licenciamentoService;

        public GrupoService(ActiveDirectoryDbContext context, LicenciamentoService licenciamentoService)
        {
            _context = context;
            _licenciamentoService = licenciamentoService;
        }

        public async Task AdicionarUsuarioAoGrupo(Guid usuarioId, Guid grupoId)
        {
            var existe = _context.UsuarioGrupos
                .Any(x => x.UsuarioId == usuarioId && x.GrupoId == grupoId);

            if (existe) return;

            var usuarioGrupo = new UsuarioGrupo(usuarioId, grupoId);
            _context.UsuarioGrupos.Add(usuarioGrupo);
            await _licenciamentoService.AtribuirLicencasPorGrupo(usuarioId, grupoId);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverUsuarioDoGrupo(Guid usuarioId, Guid grupoId)
        {
            var relacao = _context.UsuarioGrupos
                .FirstOrDefault(x => x.UsuarioId == usuarioId && x.GrupoId == grupoId);

            if (relacao == null) return;

            _context.UsuarioGrupos.Remove(relacao);
            await _licenciamentoService.RemoverLicencasPorGrupo(usuarioId, grupoId);
            await _context.SaveChangesAsync();
        }
        public async Task AtribuirGruposPorUnidadeOrganizacional(Guid usuarioId,Guid unidadeOrganizacionalId)
        {
            var gruposDaOu = _context.UnidadeOrganizacionalGrupos
                .Where(x => x.UnidadeOrganizacionalId == unidadeOrganizacionalId)
                .Select(x => x.GrupoId)
                .ToList();
            
            foreach (var grupoId in gruposDaOu)
            {
                var jaExiste = _context.UsuarioGrupos
                    .Any(x => x.UsuarioId == usuarioId && x.GrupoId == grupoId);

                if (jaExiste) continue;

                var usuarioGrupo = new UsuarioGrupo(usuarioId, grupoId);
                _context.UsuarioGrupos.Add(usuarioGrupo);

                await _licenciamentoService.AtribuirLicencasPorGrupo(usuarioId, grupoId);
            }
            await _context.SaveChangesAsync();
        }
    }
}