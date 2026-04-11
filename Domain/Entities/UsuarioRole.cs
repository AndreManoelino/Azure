using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class UsuarioRole : BaseEntity
    {
        public Guid UsuarioId { get; protected set; }

        public Usuario? Usuario { get; protected set; }

        public Guid RoleId { get; protected set; }

        public Role? Role { get; protected set; }

        public DateTime DataAssociacao { get; protected set; }

        protected UsuarioRole()
        {
        }

        public UsuarioRole(Guid usuarioId, Guid roleId)
        {
            UsuarioId = usuarioId;
            RoleId = roleId;
            DataAssociacao = DateTime.UtcNow;
        }
    }
}