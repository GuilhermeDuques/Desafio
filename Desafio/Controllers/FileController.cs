using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly FileService _fileService;

    public FileController(FileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Arquivo vazio!");

        var extension = Path.GetExtension(file.FileName).ToLower();
        if (extension == ".exe" || extension == ".bat")
            return BadRequest("Esse tipo de arquivo não é permitido.");

        if (file.Length > 10 * 1024 * 1024)
            return BadRequest("Arquivo maior que o limite de 10 MB.");

        var filePath = await _fileService.SaveFileAsync(file.FileName, file.OpenReadStream());
        return Ok(new { path = filePath });
    }


    [HttpGet("download")]
    public IActionResult DownloadFile([FromQuery] string fileName)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
        var fileStream = _fileService.GetFile(filePath);

        if (fileStream == null)
            return NotFound("Arquivo não encontrado!");

        return File(fileStream, "application/octet-stream", fileName);
    }
}
