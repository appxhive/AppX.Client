using AppX.Client.Domain.Interfaces.BlobStorage;
using AppX.Client.Infrastructure.Configuration.BlobStorage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;

namespace AppX.Client.Infrastructure.Repositories.BlobStorage
{
    public class BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettingsOptions) : IBlobStorageService
    {
        private readonly BlobStorageSettings _blobStorageSettingsOptions = blobStorageSettingsOptions.Value;

        public async Task<string> UploadToBlobAsync(Stream data, string fileName)
        {
            var blobServiceClient = new BlobServiceClient(_blobStorageSettingsOptions.ConnectionString);

            var containerClient = blobServiceClient.GetBlobContainerClient(_blobStorageSettingsOptions.Container);

            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(data);

            var blobUrl = blobClient.Uri.ToString();

            return blobUrl;
        }

        public string? GetBlobSasUrl(string? blobUrl)
        {
            if (blobUrl == null) return null;

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _blobStorageSettingsOptions.Container,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(30),
                BlobName = GetBlobNameFromUrl(blobUrl)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var blobServiceClient = new BlobServiceClient(_blobStorageSettingsOptions.ConnectionString);

            var sasToken = sasBuilder
                .ToSasQueryParameters(new Azure.Storage.StorageSharedKeyCredential(blobServiceClient.AccountName, _blobStorageSettingsOptions.AccountKey))
                .ToString();

            return $"{blobUrl}?{sasToken}";
        }

        private string GetBlobNameFromUrl(string blobUrl)
        {
            var uri = new Uri(blobUrl);
            return uri.Segments.Last();
        }
    }
}
