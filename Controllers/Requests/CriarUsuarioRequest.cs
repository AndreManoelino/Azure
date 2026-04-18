using System.Runtime.InteropServices;

namespace CorporateIdentityManager.Controllers.Requests
{
    public class CriarUsuarioRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Sobrenome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        //public Guid OrganizacaoId { get; set; }
       // public Guid DepartamentoId { get; set; }
        //public Guid UnidadeOrganizacionalId { get; set; }
        public string OrganizacaoNome { get; set; } = string.Empty;
        public string DepartamentoNome { get; set; } = string.Empty;
        public string UnidadeNome { get; set; } = string.Empty;

    }
}