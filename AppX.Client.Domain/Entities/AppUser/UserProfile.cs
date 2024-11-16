using Microsoft.AspNetCore.Identity;

namespace AppX.Client.Domain.Entities.AppUser
{
    //This extends the properties/columns of AspnetUser table
    public class UserProfile : IdentityUser
    {
        //extend custom properties:
        public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Nationality { get; set; }
        public DateTime CreatedAt { get; set; }
        //public List<string>? ProfilePictures { get; set; }
        //public string? PrimaryPicture { get; set; }
    }
}
