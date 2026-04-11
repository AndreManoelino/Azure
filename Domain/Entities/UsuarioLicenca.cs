using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class UsuarioLicenca : BaseEntity
    {
        public Guid UsuarioId { get; protected set; }
        public Usuario? Usuario { get; protected set; }

        public Guid LicencaId { get; protected set; }
        public Licenca? Licenca { get; protected set; }

        public Guid? GrupoOrigemId { get; protected set; }
        public Grupo? GrupoOrigem { get; protected set; }

        public DateTime DataAtribuicao { get; protected set; }

        public bool Manual { get; protected set; }
        public bool HerdadaDeGrupo { get; protected set; }
        public Guid? GrupoId { get; protected set; }

        protected UsuarioLicenca() { }

        public UsuarioLicenca(Guid usuarioId, Guid licencaId, bool herdadaDeGrupo, Guid? grupoOrigemId = null)
        {
            UsuarioId = usuarioId;
            LicencaId = licencaId;
            HerdadaDeGrupo = herdadaDeGrupo;
            GrupoOrigemId = grupoOrigemId;
            DataAtribuicao = DateTime.UtcNow;
        }
    }
}