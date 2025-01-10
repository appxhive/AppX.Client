using System.ComponentModel.DataAnnotations.Schema;

namespace AppX.Client.Domain.Entities.UserAccount
{
    //[Table("Photo", Schema = "Client")]
    public class Photo
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Modified_At { get; set; }
    }
}
