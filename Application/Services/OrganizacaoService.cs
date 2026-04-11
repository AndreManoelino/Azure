using CorporateIdentityManager.Domain.Entities;
using CorporateIdentityManager.Persistence.Context;
using System;
using System.Threading.Tasks;

namespace CorporateIdentityManager.Application.Services
{
    public class OrganizacaoService
    {
        private readonly ActiveDirectoryDbContext _context;

        public OrganizacaoService(ActiveDirectoryDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CriarOrganizacao(string nome, string cnpj, string dominio, string tenantId)
        {
            var organizacao = new Organizacao(nome, cnpj, dominio, tenantId);

            _context.Organizacoes.Add(organizacao);

            await _context.SaveChangesAsync();

            return organizacao.Id;
        }
    }
}