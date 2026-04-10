using System.Runtime.CompilerServices;
using CorporateIdentityManager.Domain.Abstracts;

namespace CorporateIdentityManager.Domain.Entities
{
    public class Endereco : BaseEntity
    {
        public string CEP    { get; protected set; } = string.Empty;
        public string Rua    { get; protected set; } = string.Empty;
        public string Numero { get; protected set; } = string.Empty;
        public string Bairro { get; protected set; } = string.Empty;
        public string Cidade { get; protected set; } = string.Empty;
        public string Estado { get; protected set; } = string.Empty;
        public string Pais   { get; protected set; } = string.Empty;
        public string Complemento { get; protected set; } = string.Empty;
        protected Endereco(){}
        public Endereco(string cep, string rua, string numero, string bairro,
            string cidade, string estado, string pais, string complemento)
        {
            CEP    = cep;
            Rua    = rua;
            Numero = numero;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Pais   = pais;
            Complemento = complemento;

        }
    }
}