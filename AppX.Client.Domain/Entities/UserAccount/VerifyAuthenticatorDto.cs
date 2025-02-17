namespace AppX.Client.Domain.Entities.UserAccount
{
    public class VerifyAuthenticatorDto
    {
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
        public string Code { get; set; }
    }
}