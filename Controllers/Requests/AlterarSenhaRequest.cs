namespace CorporateIdentityManager.Controllers.Requests
{
    public class AlterarSenhaRequest
    {
        public string Upn { get; set; } = string.Empty;
        public string SenhaAtual { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
    }
}