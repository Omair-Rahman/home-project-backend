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
        [RequestSizeLimit(2_147_483_648)] // Support up to ~2GB
        public async Task<IActionResult> UploadWithPreview(MediaContentInDto request)
        {
            var response = await _mediaContentService.UploadWithPreviewV2(request);

            if (!response.Status)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("upload/imgae")]
        [RequestSizeLimit(2_147_483_648)] // Support up to ~2GB
        public async Task<IActionResult> UploadImage(MediaContentInDto request)
        {
            var response = await _mediaContentService.UploadImage(request);

            if (!response.Status)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("contents")]
        public async Task<IActionResult> GetContents([FromQuery] MediaContentFilter request)
        {
            var response = await _mediaContentService.GetContents(request);

            if (!response.Status)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("image/contents")]
        public async Task<IActionResult> GetImageContents([FromQuery] MediaContentFilter request)
        {
            var response = await _mediaContentService.GetImageContents(request);

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
            return Ok(response);
        }

        [HttpPut("{id}/isFavourite")]
        public async Task<IActionResult> UpdateContentIsFavourite(int id, [FromBody] bool isFavourite)
        {
            var response = await _mediaContentService.UpdateContentIsFavourite(id, isFavourite);

            if (!response.Status)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}/rating")]
        public async Task<IActionResult> UpdateContentRating(int id, [FromBody] int rating)
        {
            var response = await _mediaContentService.UpdateContentRating(id, rating);

            if (!response.Status)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
