using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class Candidato : BaseEntity
    {
        public string NomeCompleto { get; protected set; } = string.Empty;
        public string Email { get; protected set; } = string.Empty;
        public string Telefone { get; protected set; } = string.Empty;
        public string CurriculoTexto { get; protected set; } = string.Empty;
        public Guid VagaId { get; protected set; }
        public Vaga? Vaga { get; protected set; }
        public string StatusProcesso { get; protected set; } = "Em Analise";

        protected Candidato() { }

        public Candidato(string nomeCompleto, string email, string telefone, string curriculoTexto, Guid vagaId)
        {
            NomeCompleto = nomeCompleto;
            Email = email;
            Telefone = telefone;
            CurriculoTexto = curriculoTexto;
            VagaId = vagaId;
        }

        public void AtualizarStatus(string novoStatus)
        {
            StatusProcesso = novoStatus;
            Atualizar();
        }
    }
}
