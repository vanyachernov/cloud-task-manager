using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace CloudTaskManager.Infrastructure.AzureBlob;

public class AzureBlobService
{
    private readonly BlobContainerClient _containerClient;

    public AzureBlobService(IConfiguration configuration)
    {
        var connectionString = configuration["StorageAccountConfiguration:ConnectionString"];
        var containerName = configuration["StorageAccountConfiguration:ContainerName"];

        _containerClient = new BlobContainerClient(connectionString, containerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream?> DownloadAsync(string fileName)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        if (await blobClient.ExistsAsync())
        {
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }
        return null;
    }

    public async Task<bool> DeleteAsync(string fileName)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        return await blobClient.DeleteIfExistsAsync();
    }
}