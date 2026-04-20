using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class FotoPerfil : BaseEntity
    {
        public string Base64Content { get; protected set; } = string.Empty;
        public string TipoSessao { get; protected set; } = string.Empty;
        public Guid UsuarioId { get; protected set; }
        public Usuario? Usuario { get; protected set; }

        protected FotoPerfil() { }

        public FotoPerfil(string base64Content, string tipoSessao, Guid usuarioId)
        {
            Base64Content = base64Content;
            TipoSessao = tipoSessao;
            UsuarioId = usuarioId;
        }
    }
}
