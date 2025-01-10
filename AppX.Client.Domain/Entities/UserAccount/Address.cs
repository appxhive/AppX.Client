using System.ComponentModel.DataAnnotations.Schema;

namespace AppX.Client.Domain.Entities.UserAccount
{
    //[Table("Address", Schema = "Client")]
    public class Address
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
    }
}
