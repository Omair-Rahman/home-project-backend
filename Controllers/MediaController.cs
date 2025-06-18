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
            var response = await _mediaContentService.UploadWithPreviewV2(request);

            if (!response.Status)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("contents")]
        public async Task<IActionResult> GetContents()
        {
            var response = await _mediaContentService.GetContents();

            if (!response.Status)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("contents/{profileId}")]
        public async Task<IActionResult> GetContentsByProfileId(int profileId)
        {
            var response = await _mediaContentService.GetContentsByProfileId(profileId);

            if (!response.Status)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("full/content/{id}")]
        public async Task<IActionResult> GetFull(int id)
        {
            var response = await _mediaContentService.GetFullContentsById(id);

            if (!response.Status)
            {
                return BadRequest(response);
            }
            return File(response.Data!.FullData!, response.Data.ContentType);
        }

        [HttpPut("{id}/{isFavourite}")]
        public async Task<IActionResult> UpdateContentIsFavourite(int id, bool isFavourite)
        {
            var response = await _mediaContentService.UpdateContentIsFavourite(id, isFavourite);

            if (!response.Status)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
