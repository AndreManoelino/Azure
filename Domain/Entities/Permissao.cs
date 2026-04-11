using CorporateIdentityManager.Domain.Abstracts;
namespace CorporateIdentityManager.Domain.Entities
{
    public class Permissao : BaseEntity
    {
        public string Nome { get; protected set; } = string.Empty;
        public string Descricao { get; protected set; } = string.Empty;
        public string Codigo { get; protected set; } = string.Empty;

        protected Permissao() { }
        public Permissao(string nome, string descricao, string codigo)
        {
            Nome = nome; Descricao = descricao; Codigo = codigo;
        }
    }
}