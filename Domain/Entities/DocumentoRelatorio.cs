using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class DocumentoRelatorio : BaseEntity
    {
        public string Titulo { get; protected set; } = string.Empty;
        public string Conteudo { get; protected set; } = string.Empty;
        public string DepartamentoRelacionado { get; protected set; } = string.Empty;
        public bool Aprovado { get; protected set; }
        public Guid UsuarioId { get; protected set; }
        public Usuario? Usuario { get; protected set; }
        
        protected DocumentoRelatorio() { }
        
        public DocumentoRelatorio(string titulo, string conteudo, string departamentoRelacionado, Guid usuarioId)
        {
            Titulo = titulo;
            Conteudo = conteudo;
            DepartamentoRelacionado = departamentoRelacionado;
            UsuarioId = usuarioId;
            Aprovado = false;
        }

        public void Aprovar()
        {
            Aprovado = true;
            Atualizar();
        }
    }
}
