using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PdfConverter.Core.Abstractions;

namespace PdfConverter.Core.Services;
/// <summary>
/// Cчитыватель pdf файлов
/// </summary>
public class ReaderService : IReaderService
{
    private readonly ILogger<ReaderService> _logger;

    /// <summary>
    /// Конструктор сервиса
    /// </summary>
    public ReaderService(ILogger<ReaderService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Прочитать файл и вернуть его текст
    /// </summary>
    /// <param name="webRootPath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<string>> ReadFilesAsync(IEnumerable<IFormFile> files, string webRootPath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        List<string> filePaths = await SaveFiles(files, webRootPath, cancellationToken);
        List<string> filesTexts = new();
        PDFParser pdfParser = new();
        foreach (var filePath in filePaths)
        {
            string fileText = pdfParser.ExtractText(filePath);
            filesTexts.Add(fileText);
        }
        return filesTexts;
    }

    public async Task<string> ReadFileAsync(IFormFile file, string webRootPath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        string filePath = await SaveFile(file, webRootPath, cancellationToken);
        PDFParser pdfParser = new();
        string fileText = pdfParser.ExtractText(filePath);
        return fileText;
    }

    /// <summary>
    /// Сохранить файлы локально
    /// </summary>
    /// <param name="files"></param>
    /// <param name="webRootPath"></param>
    /// <returns></returns>
    private async Task<List<string>> SaveFiles(IEnumerable<IFormFile> files, string webRootPath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (files == null)
            throw new ArgumentNullException(nameof(files));
        if (string.IsNullOrEmpty(webRootPath))
            throw new ArgumentNullException(nameof(webRootPath));

        // Список путей сохранения файлов
        List<string> filesPaths = new();

        foreach (var file in files.Where(x => x.Length > 0))
        {
            string uploads = Path.Combine(webRootPath, "SaveFiles");

            Random rnd = new();
            string filePath = Path.Combine(uploads, DateTime.Now.ToString("dd.MM.yyyy.mm.ss") + rnd.Next(1, 10000).ToString() + file.FileName);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream, cancellationToken);
            }

            filesPaths.Add(filePath);
        }

        return filesPaths;
    }

    private async Task<string> SaveFile(IFormFile file, string webRootPath, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (file == null)
            throw new ArgumentNullException(nameof(file));
        if (string.IsNullOrEmpty(webRootPath))
            throw new ArgumentNullException(nameof(webRootPath));

        string uploads = Path.Combine(webRootPath, "SaveFiles");

        Random rnd = new();
        string filePath = Path.Combine(uploads, DateTime.Now.ToString("dd.MM.yyyy.mm.ss") + rnd.Next(1, 10000).ToString() + file.FileName);

        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream, cancellationToken);
        }
        return filePath;
    }
}

