using System.ComponentModel.DataAnnotations;

namespace AppX.Client.Domain.Entities.UserAccount
{
    public class ResetPasswordDto
    {
        public string Code { get; set; }
        //Comment out Email field as now passing userid as parameter and userid 
        //is part of code which originally generated from generate reset password token

        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
