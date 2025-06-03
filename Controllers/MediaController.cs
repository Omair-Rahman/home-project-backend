using HomeProject.Models.Request.MediaContent;
using HomeProject.Services.MediaContentService;
using Microsoft.AspNetCore.Mvc;

namespace HomeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaContentService _mediaContentService;

        public MediaController(IMediaContentService mediaContentService)
        {
            _mediaContentService = mediaContentService;
        }

        [HttpPost("upload")]
        [RequestSizeLimit(1_500_000_000)] // Support up to ~1.5GB
        public async Task<IActionResult> UploadWithPreview(MediaContentInDto request)
        {
            var response = await _mediaContentService.UploadWithPreview(request);

            if (!response.Status)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

    }
}
