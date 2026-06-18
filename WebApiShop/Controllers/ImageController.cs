using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
[EnableRateLimiting("standard")]
[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public ImagesController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("קובץ לא תקין");

        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        string filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        string dbPath = Path.Combine("uploads", fileName);
        return Ok(new { path = dbPath });
    }
}