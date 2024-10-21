using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace FileContainerNet.AzureStorage
{
    public class AzureBlobFileContainer : IFileContainer
    {
        private readonly BlobServiceClient _blobService;
        private BlobContainerClient _currentContainer;

        public AzureBlobFileContainer(string connectionString) : this(connectionString, "default")
        {
        }

        public AzureBlobFileContainer(string connectionString, string defaultContainer)
        {
            _blobService = new BlobServiceClient(connectionString);
            _currentContainer = _blobService.GetBlobContainerClient(defaultContainer);
            _currentContainer.CreateIfNotExists();
            _currentContainer.SetAccessPolicy(PublicAccessType.None);
        }

        public void UseContainer(string container)
        {
            if (string.IsNullOrWhiteSpace(container))
                throw new ArgumentException("Container cannot be empty");

            var blobContainer = _blobService.GetBlobContainerClient(container);
            _currentContainer.CreateIfNotExists();
            _currentContainer.SetAccessPolicy(PublicAccessType.Blob);

            _currentContainer = blobContainer;
        }

        public void DeleteFile(string path)
        {
            var blob = _currentContainer.GetBlobClient(path);
            blob.Delete();
        }

        public async Task DeleteFileAsync(string path)
        {
            var blob = _currentContainer.GetBlobClient(path);

            await blob.DeleteAsync();
        }

        public byte[] RetreiveFile(string path)
        {
            var blob = _currentContainer.GetBlobClient(path);

            return blob.DownloadContent().Value.Content.ToArray();
        }

        public async Task<byte[]> RetreiveFileAsync(string path)
        {
            var blob = _currentContainer.GetBlobClient(path);

            return (await blob.DownloadContentAsync()).Value.Content.ToArray();
        }

        public Uri StoreFile(string path, byte[] content)
        {
            var blob = _currentContainer.GetBlobClient(path);

            var data = new BinaryData(content);
            blob.Upload(data, true);

            return blob.Uri;
        }

        public async Task<Uri> StoreFileAsync(string path, byte[] content)
        {
            var blob = _currentContainer.GetBlobClient(path);

            var data = new BinaryData(content);
            await blob.UploadAsync(data, true);

            return blob.Uri;
        }

        public Uri StoreFile(string path, Stream contentStream)
        {
            var blob = _currentContainer.GetBlobClient(path);

            blob.Upload(contentStream, true);

            return blob.Uri;
        }

        public Stream RetreiveFileStream(string path)
        {
            var blob = _currentContainer.GetBlobClient(path);

            return blob.OpenRead();
        }

        public async Task<Uri> StoreFileAsync(string path, Stream contentStream)
        {
            var blob = _currentContainer.GetBlobClient(path);

            await blob.UploadAsync(contentStream, true);

            return blob.Uri;
        }

        public async Task<Stream> RetreiveFileStreamAsync(string path)
        {
            var blob = _currentContainer.GetBlobClient(path);

            return await blob.OpenReadAsync();
        }

        public string GetBlobUriWithSasToken(string uriString)
        {
            var containerUri = new Uri(_currentContainer.Uri + "/");

            var uri = new Uri(uriString);

            var blob = _currentContainer.GetBlobClient(containerUri.MakeRelativeUri(uri).ToString());
            return blob.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(1))
                .ToString();
        }
    }
}
