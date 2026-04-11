using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class GrupoLicenca : BaseEntity
    {
        public Guid GrupoId { get; protected set; }
        public Grupo? Grupo { get; protected set; }

        public Guid LicencaId { get; protected set; }
        public Licenca? Licenca { get; protected set; }

        public DateTime DataAssociacao { get; protected set; }

        public bool Obrigatoria { get; protected set; }

        protected GrupoLicenca() { }

        public GrupoLicenca(Guid grupoId, Guid licencaId, bool obrigatoria = true)
        {
            GrupoId = grupoId;
            LicencaId = licencaId;
            DataAssociacao = DateTime.UtcNow;
            Obrigatoria = obrigatoria;
        }
    }
}