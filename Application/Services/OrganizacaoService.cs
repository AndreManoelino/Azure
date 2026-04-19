using CorporateIdentityManager.Persistence.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using CorporateIdentityManager.Domain.Entities;
using CorporateIdentityManager.Domain.Enums;
namespace CorporateIdentityManager.Application.Services
{
    public class OrganizacaoService(ActiveDirectoryDbContext context, GrupoService grupoService)
    {
        private readonly ActiveDirectoryDbContext _context = context;
        private readonly GrupoService _grupoService = grupoService;

        public async Task<Guid> CriarOrganizacao(string nome, string cnpj, string dominio, string tenantId)
        {
            var organizacao = new Organizacao(nome, cnpj, dominio, tenantId);

            _context.Organizacoes.Add(organizacao);
            await _context.SaveChangesAsync();

            return organizacao.Id;
        }

        public async Task AplicarRegrasDeEntradaUsuario(Guid usuarioId, Guid unidadeOrganizacionalId)
        {
            await _grupoService.AtribuirGruposPorUnidadeOrganizacional(usuarioId, unidadeOrganizacionalId);

            var ou = _context.UnidadesOrganizacionais
                .FirstOrDefault(x => x.Id == unidadeOrganizacionalId);

            if (ou == null) return;

            if (ou.Nome == "BackOffice")
            {
                var grupo = _context.Grupos
                    .FirstOrDefault(g => g.TipoGrupo == Domain.Enums.TipoGrupo.M365_E5);

                if (grupo != null)
                {
                    await _grupoService.AdicionarUsuarioAoGrupo(usuarioId, grupo.Id);
                }
            }

            if (ou.Nome == "Atendimento")
            {
                var grupo = _context.Grupos
                    .FirstOrDefault(g => g.TipoGrupo == Domain.Enums.TipoGrupo.F5);

                if (grupo != null)
                {
                    await _grupoService.AdicionarUsuarioAoGrupo(usuarioId, grupo.Id);
                }
            }
        }
    }
}