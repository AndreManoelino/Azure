using CorporateIdentityManager.Domain.Entities;
using CorporateIdentityManager.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CorporateIdentityManager.Application.Services
{
    public class RelatorioService(ActiveDirectoryDbContext context)
    {
        private readonly ActiveDirectoryDbContext _context = context;

        public async Task<Guid> CriarRelatorio(Guid usuarioId, string titulo, string conteudo, string departamento)
        {
            var relatorio = new DocumentoRelatorio(titulo, conteudo, departamento, usuarioId);
            _context.DocumentosRelatorios.Add(relatorio);
            await _context.SaveChangesAsync();
            return relatorio.Id;
        }

        public async Task<IEnumerable<DocumentoRelatorio>> ListarPorDepartamento(string departamento)
        {
            return await _context.DocumentosRelatorios
                .Include(r => r.Usuario)
                .Where(r => r.DepartamentoRelacionado == departamento)
                .ToListAsync();
        }

        public async Task AprovarRelatorio(Guid relatorioId)
        {
            var relatorio = await _context.DocumentosRelatorios.FindAsync(relatorioId);
            if (relatorio != null)
            {
                relatorio.Aprovar();
                await _context.SaveChangesAsync();
            }
        }
    }
}
