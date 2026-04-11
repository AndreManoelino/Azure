using CorporateIdentityManager.Persistence.Context;
using CorporateIdentityManager.Domain.Entities;
using System.Threading.Tasks;
namespace CorporateIdentityManager.Application.Services
{
    public class LicenciamentoService
    {
        private readonly ActiveDirectoryDbContext _context;
        public LicenciamentoService(ActiveDirectoryDbContext context)
        {
            _context = context;
        }
        public async Task AtribuirLicencaDireta(Guid usuarioId, Guid licencaId)
        {
            var existe = _context.UsuarioLicencas
                .Any(x => x.UsuarioId == usuarioId && x.LicencaId == licencaId);

            if (existe)
                return;

            var usuarioLicenca = new UsuarioLicenca(
                usuarioId,
                licencaId,
                false,
                null
            );

            _context.UsuarioLicencas.Add(usuarioLicenca);

            await _context.SaveChangesAsync();
        }
        public async Task AtribuirLicencasPorGrupo(Guid usuarioId, Guid grupoId)
        {
            var licencasDoGrupo = _context.GrupoLicencas
                .Where(x => x.GrupoId == grupoId)
                .Select(x => x.LicencaId)
                .ToList();

            foreach (var licencaId in licencasDoGrupo)
            {
                var jaPossui = _context.UsuarioLicencas
                    .Any(x => x.UsuarioId == usuarioId && x.LicencaId == licencaId);

                if (jaPossui)
                    continue;

                var usuarioLicenca = new UsuarioLicenca(
                    usuarioId,
                    licencaId,
                    true,
                    grupoId
                );

                _context.UsuarioLicencas.Add(usuarioLicenca);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoverLicencasPorGrupo(Guid usuarioId, Guid grupoId)
        {
            var licencas = _context.UsuarioLicencas
                .Where(x => x.UsuarioId == usuarioId && x.GrupoOrigemId == grupoId && x.HerdadaDeGrupo);

            _context.UsuarioLicencas.RemoveRange(licencas);

            await _context.SaveChangesAsync();
        }
    }
}