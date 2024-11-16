namespace AppX.Client.Infrastructure.Configuration.BlobStorage
{
    public class BlobStorageSettings
    {
        public string ConnectionString { get; set; } = default!;
        public string Container { get; set; } = default!;
        public string AccountKey { get; set; } = default!;
    }
}
