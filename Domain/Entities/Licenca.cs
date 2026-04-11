using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class Licenca : BaseEntity
    {
        public string Nome { get; protected set; } = string.Empty;

        public string Sku { get; protected set; } = string.Empty;

        public string Descricao { get; protected set; } = string.Empty;

        public int QuantidadeDisponivel { get; protected set; }

        public int QuantidadeConsumida { get; protected set; }

        public bool Ativa { get; protected set; }
        public ICollection<GrupoLicenca> GrupoLicencas { get; protected set; } = new List<GrupoLicenca>();

        protected Licenca() { }

        public Licenca(
            string nome,
            string sku,
            string descricao,
            int quantidadeDisponivel)
        {
            Nome = nome;
            Sku = sku;
            Descricao = descricao;
            QuantidadeDisponivel = quantidadeDisponivel;
            QuantidadeConsumida = 0;
            Ativa = true;
        }
    }
}