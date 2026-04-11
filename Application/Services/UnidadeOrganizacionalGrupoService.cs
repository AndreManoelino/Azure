using CorporateIdentityManager.Domain.Entities;
using CorporateIdentityManager.Persistence.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CorporateIdentityManager.Application.Services
{
    public class UnidadeOrganizacionalGrupoService
    {
        private readonly ActiveDirectoryDbContext _context;

        public UnidadeOrganizacionalGrupoService(ActiveDirectoryDbContext context)
        {
            _context = context;
        }

        public async Task VincularGrupo(Guid unidadeOrganizacionalId, Guid grupoId)
        {
            var existe = _context.UnidadeOrganizacionalGrupos
                .Any(x => x.UnidadeOrganizacionalId == unidadeOrganizacionalId
                       && x.GrupoId == grupoId);

            if (existe) return;

            var relacao = new UnidadeOrganizacionalGrupo(unidadeOrganizacionalId, grupoId);

            _context.UnidadeOrganizacionalGrupos.Add(relacao);

            await _context.SaveChangesAsync();
        }

        public async Task RemoverGrupo(Guid unidadeOrganizacionalId, Guid grupoId)
        {
            var relacao = _context.UnidadeOrganizacionalGrupos
                .FirstOrDefault(x => x.UnidadeOrganizacionalId == unidadeOrganizacionalId
                                  && x.GrupoId == grupoId);

            if (relacao == null) return;

            _context.UnidadeOrganizacionalGrupos.Remove(relacao);

            await _context.SaveChangesAsync();
        }
    }
}