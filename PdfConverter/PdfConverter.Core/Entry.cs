using Microsoft.Extensions.DependencyInjection;
using PdfConverter.Core.Abstractions;
using PdfConverter.Core.Services;

namespace PdfConverter.Core;

/// <summary>
/// Регистрация зависимостей проекта
/// </summary>
public static class Entry
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddScoped<IReaderService, ReaderService>();
        return services;
    }
}
