using FileContainerNet.Injection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileContainerNet.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddFileContainer(this IServiceCollection collection, Func<FileContainerBuilder, IFileContainer> containerBuilder)
        {
            var builder = new FileContainerBuilder();

            return collection.AddScoped(s => containerBuilder(builder));
        }
    }
}
