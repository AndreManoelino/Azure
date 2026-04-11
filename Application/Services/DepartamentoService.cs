using CorporateIdentityManager.Persistence.Context;
using CorporateIdentityManager.Domain.Entities;
using System;
using System.Threading.Tasks;
namespace CorporateIdentityManager.Application.Services
{
    public class DepartamentoService
    {
        private readonly ActiveDirectoryDbContext _context;
        public DepartamentoService(ActiveDirectoryDbContext context)
        {
            _context = context;
        }
        public async Task<Guid> CriarDepartamento(string nome, string descricao, Guid organizacaoId)
        {
            var departamento = new Departamento(nome, descricao, organizacaoId);
            _context.Departamentos.Add(departamento);
            await _context.SaveChangesAsync();
            return departamento.Id;
        }
    }
}