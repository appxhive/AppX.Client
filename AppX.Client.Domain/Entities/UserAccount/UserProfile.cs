using AppX.Client.Domain.Entities.Client;
using Microsoft.AspNetCore.Identity;

namespace AppX.Client.Domain.Entities.UserAccount
{
    //This extends the properties/columns of AspnetUser table
    public class UserProfile : IdentityUser
    {
        public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Gender { get; set; }
        public string? PrimaryRole { get; set; }
        //Foreign Key to ClientProfile
        public Guid? ClientProfileId { get; set; }
        public ClientProfile ClientProfile { get; set; } = default!;
        public DateOnly? BirthDate { get; set; }
        public string? Nationality { get; set; }
        public DateTime CreatedAt { get; set; }

        //public List<string>? ProfilePictures { get; set; }
        //public string? PrimaryPicture { get; set; }
    }
}
