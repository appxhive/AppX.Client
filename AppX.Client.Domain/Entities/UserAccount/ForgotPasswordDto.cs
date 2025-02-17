using System.ComponentModel.DataAnnotations;

namespace AppX.Client.Domain.Entities.UserAccount
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
