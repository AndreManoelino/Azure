using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class Organizacao : BaseEntity
    {
        public string Nome { get; protected set; } = string.Empty;
        public string CNPJ { get; protected set; } = string.Empty;
        public string DominioPrincipal { get; protected set; } = string.Empty;
        public string TenantId { get; protected set; } = string.Empty;
        protected Organizacao() {}
        public Organizacao (string nome, string cnpj, string dominioPrincipal, string tenantId)
        {
            Nome = nome;
            CNPJ = cnpj;
            DominioPrincipal = dominioPrincipal;
            TenantId = tenantId;
        }
    }
}