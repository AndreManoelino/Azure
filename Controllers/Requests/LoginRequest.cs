namespace CorporateIdentityManager.Controllers.Requests
{
    public class LoginRequest
    {
        public string Upn { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
    }
}