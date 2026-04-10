namespace CorporateIdentityManager.Domain.Abstracts
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();

        public DateTime DataCriacao { get; protected set; } = DateTime.UtcNow;

        public DateTime? DataAtualizacao { get; protected set; }

        public bool Ativo { get; protected set; } = true;

        public bool Excluido { get; protected set; } = false;

        public void Atualizar()
        {
            DataAtualizacao = DateTime.UtcNow;
        }

        public void Desativar()
        {
            Ativo = false;
        }

        public void Excluir()
        {
            Excluido = true;
            Ativo = false;
        }
    }
}