using CorporateIdentityManager.Domain.Abstracts;
namespace CorporateIdentityManager.Domain.Entities
{
    public class UnidadeOrganizacionalGrupo : BaseEntity
    {
        public Guid UnidadeOrganizacionalId { get; protected set; }
        public UnidadeOrganizacional? UnidadeOrganizacional { get; protected set; }
        public Guid GrupoId { get; protected set; }
        public Grupo? Grupo { get; protected set; }
        protected UnidadeOrganizacionalGrupo() { }
        public UnidadeOrganizacionalGrupo(Guid unidadeOrganizacionalId, Guid grupoId)
        {
            UnidadeOrganizacionalId = unidadeOrganizacionalId;
            GrupoId = grupoId;

        }
    }
}