using CorporateIdentityManager.Domain.Entities;
using CorporateIdentityManager.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CorporateIdentityManager.Application.Services
{
    public class RecrutamentoService(ActiveDirectoryDbContext context)
    {
        private readonly ActiveDirectoryDbContext _context = context;

        public async Task<Guid> CriarVaga(string titulo, string descricao, string departamento)
        {
            var vaga = new Vaga(titulo, descricao, departamento);
            _context.Vagas.Add(vaga);
            await _context.SaveChangesAsync();
            return vaga.Id;
        }

        public async Task<IEnumerable<Vaga>> ListarVagasAbertas()
        {
            return await _context.Vagas.Where(v => v.Status == "Aberta").ToListAsync();
        }

        public async Task<Guid> AdicionarCandidato(Guid vagaId, string nome, string email, string telefone, string curriculo)
        {
            var candidato = new Candidato(nome, email, telefone, curriculo, vagaId);
            _context.Candidatos.Add(candidato);
            await _context.SaveChangesAsync();
            return candidato.Id;
        }

        public async Task<IEnumerable<Candidato>> ListarCandidatosPorVaga(Guid vagaId)
        {
            return await _context.Candidatos.Where(c => c.VagaId == vagaId).ToListAsync();
        }
    }
}
