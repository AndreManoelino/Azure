using CorporateIdentityManager.Domain.Abstracts;
using CorporateIdentityManager.Domain.Enums;

namespace CorporateIdentityManager.Domain.Entities
{
    public class Grupo : BaseEntity
    {
        public string Nome { get; protected set; } = string.Empty;
        public string Descricao { get; protected set; } = string.Empty;
        public bool GrupoDeSeguranca { get; protected set; }
        public bool GrupoDeLicenciamento { get; protected set; }
        public bool GrupoDeDistribuicao { get; protected set; }
        public ICollection<UsuarioGrupo> UsuarioGrupos { get; protected set; } = []; // Mesmo que new List();
        public ICollection<GrupoLicenca> GrupoLicencas { get; protected set; } = [];
        public TipoGrupo TipoGrupo { get; protected set; }
        protected Grupo() { }
        public Grupo (string nome, string descricao, bool grupoDeSeguranca,
            bool grupoDeLicenciamento, bool grupoDeDistribuicao,TipoGrupo tipoGrupo)
        {
            Nome = nome;
            Descricao = descricao;
            GrupoDeSeguranca = grupoDeSeguranca;
            GrupoDeLicenciamento = grupoDeLicenciamento;
            GrupoDeDistribuicao = grupoDeDistribuicao;
            TipoGrupo = tipoGrupo;

        }
    }
}