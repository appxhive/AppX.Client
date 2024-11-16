namespace AppX.Client.Domain.Interfaces.BlobStorage
{
    public interface IBlobStorageService
    {
        Task<string> UploadToBlobAsync(Stream data, string fileName);
        string? GetBlobSasUrl(string? blobUrl);
    }
}
