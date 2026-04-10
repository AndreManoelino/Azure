using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class UnidadeOrganizacional : BaseEntity
    {
        public string Nome { get; protected set; } = string.Empty;

        public string Descricao { get; protected set; } = string.Empty;

        public Guid DepartamentoId { get; protected set; }

        public Departamento? Departamento { get; protected set; }

        public Guid? UnidadePaiId { get; protected set; }

        public UnidadeOrganizacional? UnidadePai { get; protected set; }

        protected UnidadeOrganizacional() { }

        public UnidadeOrganizacional(
            string nome,
            string descricao,
            Guid departamentoId,
            Guid? unidadePaiId = null)
        {
            Nome = nome;
            Descricao = descricao;
            DepartamentoId = departamentoId;
            UnidadePaiId = unidadePaiId;
        }
    }
}