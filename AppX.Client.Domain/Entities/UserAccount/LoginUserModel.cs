using System.ComponentModel.DataAnnotations;

namespace AppX.Client.Domain.Entities.UserAccount
{
    public class LoginUserModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
