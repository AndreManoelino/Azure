using CorporateIdentityManager.Persistence.Context;
using CorporateIdentityManager.Domain.Entities;
namespace CorporateIdentityManager.Application.Services
{
    public class UnidadeOrganizacionalService
    {
        private readonly ActiveDirectoryDbContext _context;
        public UnidadeOrganizacionalService(ActiveDirectoryDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> CriarUnidadeOrganizacional(string nome, string descricao, Guid departamentoId)
        {
            var unidade = new UnidadeOrganizacional(nome, descricao, departamentoId);
            _context.UnidadesOrganizacionais.Add(unidade);
            await _context.SaveChangesAsync();
            return unidade.Id;
        }
    }
}