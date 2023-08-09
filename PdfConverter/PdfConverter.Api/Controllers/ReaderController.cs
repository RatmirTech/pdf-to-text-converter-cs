using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PdfConverter.Api.Controllers;

[Route("Reader")]
[ApiController]
[EnableCors]
public class ReaderController : ControllerBase
{
    private readonly ILogger<ReaderController> _logger;

    public ReaderController(ILogger<ReaderController> logger)
    {
        _logger = logger;
    }


    [HttpPost(nameof(ReadFile))]
    public async Task<string> ReadFile()
    {
        return "";
    }
}
