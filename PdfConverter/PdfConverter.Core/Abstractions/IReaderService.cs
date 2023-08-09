using Microsoft.AspNetCore.Http;

namespace PdfConverter.Core.Abstractions;

/// <summary>
/// Интерфейс сервиса Считывателя pdf файлов
/// </summary>
public interface IReaderService
{
    /// <summary>
    /// Прочитать pdf файл и вернуть текст файла
    /// </summary>
    /// <returns>текст из пдф файла</returns>
    Task<List<string>> ReadFilesAsync(IEnumerable<IFormFile> files, string webRootPath, CancellationToken cancellationToken);

    Task<string> ReadFileAsync(IFormFile file, string webRootPath, CancellationToken cancellationToken);
}
