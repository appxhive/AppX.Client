using Microsoft.AspNetCore.Identity;

namespace AppX.Client.Domain.Entities
{
    public class User : IdentityUser
    {
        //extend custom properties:
        public Guid? ClientId { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
        //public List<string>? ProfilePics { get; set; }
        //public string? PrimaryPic { get; set; }
    }
}
