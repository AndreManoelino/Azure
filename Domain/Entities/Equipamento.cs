using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class Equipamento : BaseEntity
    {
        public string Nome { get; protected set; } = string.Empty;
        public string Marca { get; protected set; } = string.Empty;
        public string ServiceTag { get; protected set; } = string.Empty;
        public string Tipo { get; protected set; } = string.Empty; // Notebook, Monitor, Celular
        public string Status { get; protected set; } = "Em Estoque"; // Em Estoque, Em Uso, Manutenção


        public Guid? UsuarioId { get; protected set; }
        public Usuario? Usuario { get; protected set; }

        protected Equipamento() { }

        public Equipamento(string nome, string marca, string serviceTag, string tipo)
        {
            Nome = nome;
            Marca = marca;
            ServiceTag = serviceTag;
            Tipo = tipo;
        }


        public void AlocarParaUsuario(Guid usuarioId)
        {
            UsuarioId = usuarioId;
            Status = "Em Uso";
            Atualizar();
        }

        public void DevolverParaEstoque()
        {
            UsuarioId = null;
            Status = "Em Estoque";
            Atualizar();
        }
    }
}
