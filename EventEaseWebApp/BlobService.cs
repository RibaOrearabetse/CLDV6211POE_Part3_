using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

public class BlobService
{
    private readonly BlobContainerClient _containerClient;

    public BlobService(IConfiguration configuration)
    {
        string accountName = configuration["AzureStorage:AccountName"];
        string containerName = configuration["AzureStorage:ContainerName"];
        string accountKey = configuration["AzureStorage:AccountKey"];
        string connectionString = $"DefaultEndpointsProtocol = https; AccountName = blobstorageeventeaze; AccountKey = Zkb + v2LqOmw / 6dp8Kk5IfKzd0UjNu0g7h6aYy0kAP6fXUP9 + MYonHVuFSkMNbVjrnhIlbqrPlNNO + AStvGMdGg ==; EndpointSuffix = core.windows.net";

        _containerClient = new BlobContainerClient(connectionString, containerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var blobClient = _containerClient.GetBlobClient(file.FileName);
        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);
        return blobClient.Uri.ToString();
    }
}
