using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class Vaga : BaseEntity
    {
        public string Titulo { get; protected set; } = string.Empty;
        public string Descricao { get; protected set; } = string.Empty;
        public string Departamento { get; protected set; } = string.Empty;
        public string Status { get; protected set; } = "Aberta"; // Aberta, Fechada

        protected Vaga() { }

        public Vaga(string titulo, string descricao, string departamento)
        {
            Titulo = titulo;
            Descricao = descricao;
            Departamento = departamento;
        }

        public void FecharVaga()
        {
            Status = "Fechada";
            Atualizar();
        }
    }
}
