using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class Holerite : BaseEntity
    {
        public string MesReferencia { get; protected set; } = string.Empty;
        public string AnoReferencia { get; protected set; } = string.Empty;
        public decimal ValorLiquido { get; protected set; }
        public string Descricao { get; protected set; } = string.Empty;
        
        public Guid UsuarioId { get; protected set; }
        public Usuario? Usuario { get; protected set; }

        protected Holerite() { }

        // Mágica acontecendo para o RH salvar o documento financeiro
        public Holerite(string mesReferencia, string anoReferencia, decimal valorLiquido, string descricao, Guid usuarioId)
        {
            MesReferencia = mesReferencia;
            AnoReferencia = anoReferencia;
            ValorLiquido = valorLiquido;
            Descricao = descricao;
            UsuarioId = usuarioId;
        }
    }
}
