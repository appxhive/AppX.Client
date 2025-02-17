namespace AppX.Client.Domain.Entities.UserAccount
{
    public class TwoFactorAuthentication
    {
        public string? Code { get; set; } //this should be nullable so that [HttpPost] EnableAuthenticator works correctly

        //this is the AuthenticatorKey that must be passed in from AppXHive Identity Manager API to User Mobile Authenticator App e.i.: Microsoft Authenticator
        public string? Token { get; set; } //this should be nullable so that [HttpPost] EnableAuthenticator works correctly
        public string? QRCodeUrl { get; set; } //this should be nullable so that [HttpPost] EnableAuthenticator works correctly
    }
}
