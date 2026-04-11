using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class Pessoa : BaseEntity
    {
        public string Nome { get; protected set; } = string.Empty;
        public string Sobrenome { get; protected set;} = string.Empty;
        public string Cpf { get; protected set; } = string.Empty;
        public string Email { get; protected set; } = string.Empty;
        public string Telefone { get; protected set; } = string.Empty;
        public DateTime DataNascimento { get; protected set; }
        
        public Guid? EnderecoId { get; protected set;} 
        public Endereco? Endereco { get; protected set; } 
        public Pessoa() {}
        protected Pessoa(string nome, string sobrenome, string cpf, string email,
            string telefone, DateTime dataNascimento)
        {
            Nome = nome;
            Sobrenome = sobrenome;
            Cpf = cpf;
            Email = email;
            Telefone = telefone;
            DataNascimento = dataNascimento;
        }
    }
}