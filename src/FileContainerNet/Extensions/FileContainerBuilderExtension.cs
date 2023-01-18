using FileContainerNet.AzureStorage;
using FileContainerNet.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileContainerNet.Extensions
{
    public static class FileContainerBuilderExtension
    {
        public static AzureBlobFileContainer UseAzureBlob(this FileContainerBuilder builder,
            string connectionString) => new(connectionString);
        public static AzureBlobFileContainer UseAzureBlob(this FileContainerBuilder builder, 
            string connectionString, string defaultContainer) => new(connectionString, defaultContainer);
    }
}
