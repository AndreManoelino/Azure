using CorporateIdentityManager.Domain.Abstracts;
namespace CorporateIdentityManager.Domain.Entities
{
    public class UsuarioGrupo : BaseEntity
    {
        public Guid UsuarioId { get; protected set; }
        public Usuario? Usuario { get; protected set; }
        public Guid GrupoId { get; protected set; }
        public Grupo? Grupo { get; protected set; }
        public DateTime DataAssociacao { get; protected set; }
        protected UsuarioGrupo() { }
        public UsuarioGrupo(Guid usuarioId, Guid grupoId)
        {
            UsuarioId = usuarioId;
            GrupoId = grupoId;
            DataAssociacao = DateTime.UtcNow;
        }
    }
}