using CloudTaskManager.Infrastructure.AzureBlob;
using Microsoft.AspNetCore.Mvc;

namespace CloudTaskManager.Api.Controllers;

[ApiController]
[Route("api/[action]")]
public class FileController(AzureBlobService blobService) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile? file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }
        
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        await using var stream = file.OpenReadStream();
        
        var url = await blobService.UploadAsync(stream, fileName);
        
        return Ok(new { Url = url });
    }

    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> Download(string fileName)
    {
        var stream = await blobService.DownloadAsync(fileName);
        if (stream == null)
        {
            return NotFound();
        }

        return File(stream, "application/octet-stream", fileName);
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> Delete(string fileName)
    {
        var result = await blobService.DeleteAsync(fileName);
        return result 
            ? Ok() 
            : NotFound();
    }
}