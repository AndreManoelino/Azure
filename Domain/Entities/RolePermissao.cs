using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class RolePermissao : BaseEntity
    {
        public Guid RoleId { get; protected set; }

        public Role? Role { get; protected set; }

        public Guid PermissaoId { get; protected set; }

        public Permissao? Permissao { get; protected set; }

        protected RolePermissao()
        {
        }

        public RolePermissao(Guid roleId, Guid permissaoId)
        {
            RoleId = roleId;
            PermissaoId = permissaoId;
        }
    }
}