namespace CorporateIdentityManager.Controllers.Auth.Responses
{
    public class LoginResponse
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UPN { get; set; } = string.Empty;
        public List<string> Grupos { get; set; } = [];
    }
}