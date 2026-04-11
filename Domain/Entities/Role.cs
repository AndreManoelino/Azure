using CorporateIdentityManager.Domain.Abstracts;
namespace CorporateIdentityManager.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Nome { get; protected set; } = string.Empty;
        public string Descricao { get; protected set; } = string.Empty;
        public bool RoleSistema { get; protected set; }
        public ICollection<UsuarioRole> UsuarioRoles { get; protected set; } = new List<UsuarioRole>();
        protected Role() { }
        public Role (string nome, string descricao, bool roleSistema)
        {
            Nome = nome; Descricao = descricao; RoleSistema = roleSistema;
        }

    }
}