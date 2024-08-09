using Microsoft.AspNetCore.Mvc;
using WineCollectionManagerApi.Models;
using WineCollectionManagerApi.Services;

namespace WineCollectionManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WinemakersController : ControllerBase
    {
        private readonly IWinemakerService _winemakerService;

        public WinemakersController(IWinemakerService winemakerService)
        {
            _winemakerService = winemakerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WineMakerModel>>> GetAll()
        {
            var winemakers = await _winemakerService.GetAll();
            if (winemakers == null || !winemakers.Any())
            {
                return NotFound("No winemakers found.");
            }

            return Ok(winemakers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WineMakerModel>> GetById(int id)
        {
            if (id < 0)
                return BadRequest("Winemaker ID must be a non-negative integer.");

            var winemaker = await _winemakerService.GetById(id);
            if (winemaker == null)
                return NotFound($"Winemaker with ID {id} not found.");

            return Ok(winemaker);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] WineMakerCreateModel winemakerCreate)
        {
            if (winemakerCreate == null)
                return BadRequest("Winemaker creation data is required.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var winemaker = new WineMakerModel
            {
                Name = winemakerCreate.Name
            };

            await _winemakerService.Add(winemaker);

            return CreatedAtAction(nameof(GetById), new { id = winemaker.Id }, winemaker);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] WineMakerModel winemaker)
        {
            if (id < 0)
                return BadRequest("Winemaker ID must be a non-negative integer.");

            if (winemaker == null)
                return BadRequest("Winemaker data is required.");

            if (id != winemaker.Id)
                return BadRequest("ID mismatch.");

            await _winemakerService.Update(winemaker);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id < 0)
                return BadRequest("Winemaker ID must be a non-negative integer.");

            await _winemakerService.Delete(id);

            return NoContent();
        }
    }
}
