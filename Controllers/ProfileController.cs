using HomeProject.Models.Request.Profile;
using HomeProject.Services.ProfileService;
using Microsoft.AspNetCore.Mvc;

namespace HomeProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ProfileFilter request)
        {
            var response = await _profileService.Get(request);
            if (!response.Items!.Any())
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _profileService.GetById(id);

            if (!response.Status)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ProfileInDto request)
        {
            var response = await _profileService.Create(request);

            if (!response.Status)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public Task<IActionResult> Put(int id, [FromForm] ProfileInDto request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
