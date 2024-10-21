using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileContainerNet
{
    public interface IFileContainer
    {
        void UseContainer(string container);

        Uri StoreFile(string path, Stream contentStream);
        Stream RetreiveFileStream(string path);

        Uri StoreFile(string path, byte[] content);
        byte[] RetreiveFile(string path);
        void DeleteFile(string path);

        Task<Uri> StoreFileAsync(string path, Stream contentStream);
        Task<Stream> RetreiveFileStreamAsync(string path);

        Task<Uri> StoreFileAsync(string path, byte[] content);
        Task<byte[]> RetreiveFileAsync(string path);
        Task DeleteFileAsync(string path);

        string GetBlobUriWithSasToken(string uriString);
    }
}
