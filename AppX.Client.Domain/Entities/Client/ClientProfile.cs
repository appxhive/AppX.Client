using Microsoft.EntityFrameworkCore;

namespace AppX.Client.Domain.Entities.Client
{
    public class ClientProfile
    {
        public Guid ClientProfileId { get; set; }
        public string ClientName { get; set; } = default!;
        public string? ClientCountry { get; set; }
        public string? ClientAddress { get; set; }
        public string? ClientBrandLogo { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
