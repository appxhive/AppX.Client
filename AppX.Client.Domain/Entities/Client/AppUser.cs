using AppX.Client.Domain.Entities.AppUser;

namespace AppX.Client.Domain.Entities.Client
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string? PrimaryRole { get; set; }

        //Foreign Key to UserProfile
        public string UserProfileId { get; set; } = default!; //type is string based on Identity
        public UserProfile? UserProfile { get; set; }

        //Foreign Key to ClientProfile
        public Guid? ClientProfileId { get; set; }
        public ClientProfile ClientProfile { get; set; } = default!;
    }
}
