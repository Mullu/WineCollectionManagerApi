using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using WineCollectionManagerApi.Models;
using WineCollectionManagerApi.Services;
using WineCollectionManagerApi.Enums;

namespace WineCollectionManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WineBottlesController : ControllerBase
    {
        private readonly IWineBottleService _wineBottleService;
        private readonly IMapper _mapper;

        public WineBottlesController(IWineBottleService wineBottleService, IMapper mapper)
        {
            _wineBottleService = wineBottleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WineBottleModel>>> GetAll()
        {
            var wineBottles = await _wineBottleService.GetAll();
            if (wineBottles == null || !wineBottles.Any())
            {
                return NotFound("No wine bottles found.");
            }

            return Ok(wineBottles);
        }

        [HttpGet("winemaker/{winemakerId}")]
        public async Task<ActionResult<IEnumerable<WineBottleModel>>> GetByWinemakerId(int winemakerId)
        {
            if (winemakerId < 0)
                return BadRequest("Winemaker ID must be a non-negative integer.");

            var wineBottles = await _wineBottleService.GetByWinemakerId(winemakerId);
            if (wineBottles == null || !wineBottles.Any())
            {
                return NotFound($"No wine bottles found for winemaker with ID {winemakerId}.");
            }

            return Ok(wineBottles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WineBottleModel>> GetById(int id)
        {
            if (id < 0)
                return BadRequest("Wine bottle ID must be a non-negative integer.");

            var wineBottle = await _wineBottleService.GetById(id);
            if (wineBottle == null)
                return NotFound($"Wine bottle with ID {id} not found.");

            return Ok(wineBottle);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] WineBottleCreateModel wineBottleCreate)
        {
            if (wineBottleCreate == null)
                return BadRequest("Wine bottle creation data is required.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var wineBottle = _mapper.Map<WineBottleModel>(wineBottleCreate);
            await _wineBottleService.Add(wineBottle);
            
            return CreatedAtAction(nameof(GetById), new { id = wineBottle.Id }, wineBottle);
        }

        [HttpPut("{id}")]
        public Task<ActionResult> Update(int id, [FromBody] WineBottleModel wineBottle)
        {
            if (id < 0)
                return Task.FromResult<ActionResult>(BadRequest("Wine bottle ID must be a non-negative integer."));

            if (wineBottle == null)
                return Task.FromResult<ActionResult>(BadRequest("Wine bottle data is required."));

            if (id != wineBottle.Id)
                return Task.FromResult<ActionResult>(BadRequest("ID mismatch."));

           _wineBottleService.Update(wineBottle);

            return Task.FromResult<ActionResult>(NoContent());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id < 0)
                return BadRequest("Wine bottle ID must be a non-negative integer.");

            await _wineBottleService.Delete(id);

            return NoContent();
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<WineBottleModel>>> Filter(
            [FromQuery] int? year,
            [FromQuery] int? size,
            [FromQuery] int? countInWineCellar,
            [FromQuery] WineStyle? style,
            [FromQuery] string? taste,
            [FromQuery] string? foodPairing)
        {
            var wineBottles = await _wineBottleService
                .Filter(year, size, countInWineCellar, style, taste, foodPairing);
            
            return Ok(wineBottles);
        }
    }
}
