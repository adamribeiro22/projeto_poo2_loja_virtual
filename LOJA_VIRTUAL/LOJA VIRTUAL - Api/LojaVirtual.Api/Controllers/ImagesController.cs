using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LojaVirtual.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImagensController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public ImagensController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadImagem(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Nenhum arquivo enviado.");
            }

            var uploadsDir = Path.Combine(_env.WebRootPath, "images", "variacoes");
            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var imageUrl = $"/images/variacoes/{fileName}";

            return Ok(new { url = imageUrl });
        }
    }
}
