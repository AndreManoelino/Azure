using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class Departamento : BaseEntity
    {
        public string Nome { get; protected set; } = string.Empty;
        public string Descricao { get; protected set; } = string.Empty;
        public Guid OrganizacaoId { get; protected set; }
        public Organizacao? Organizacao { get; protected set; }
        protected Departamento(){}
        public Departamento(string nome, string descricao, Guid organizacaoId)
        {
            Nome = nome;
            Descricao = descricao;
            OrganizacaoId= organizacaoId;
        }
    }
}