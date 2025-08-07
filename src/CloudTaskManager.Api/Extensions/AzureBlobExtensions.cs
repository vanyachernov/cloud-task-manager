using CloudTaskManager.Infrastructure.AzureBlob;

namespace CloudTaskManager.Api.Extensions;

public static class AzureBlobExtensions
{
    public static IServiceCollection AddAzureBlobStorage(this IServiceCollection services)
    {
        services.AddSingleton<AzureBlobService>();
        return services;
    }
}