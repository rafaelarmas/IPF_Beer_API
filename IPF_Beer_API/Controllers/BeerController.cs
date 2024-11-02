using IPF_Beer_API.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPF_Beer_API.Controllers
{
    [ApiController]
    [Route("beer")]
    public class BeerController : ControllerBase
    {
        private readonly ILogger<BeerController> _logger;
        
        private readonly Data.DataContext _context;

        public BeerController(ILogger<BeerController> logger, Data.DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Beer>> AddBeer(Beer beer)
        {
            try
            { 
                _context.Beers.Add(beer);
                await _context.SaveChangesAsync();

                return beer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddBeer failed.");
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Beer>> UpdateBeerById(int id, Beer beer)
        {
            try
            {
                var beerToUpdate = await _context.Beers
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (beerToUpdate != null)
                {
                    beerToUpdate.Name = beer.Name;
                    beerToUpdate.PercentageAlcoholByVolume = beer.PercentageAlcoholByVolume;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound(beer);
                }

                return beer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateBeerById failed.");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Beer>>> GetAllBeers([FromQuery]string? gtAlcoholByVolume, [FromQuery] string? ltAlcoholByVolume)
        {
            try
            {
                IQueryable<Beer> query = _context.Beers.AsQueryable();

                if (!string.IsNullOrWhiteSpace(gtAlcoholByVolume) && decimal.TryParse(gtAlcoholByVolume, out decimal gtAlcoholByVolumeDecimal))
                {
                    query = _context.Beers.Where(x => x.PercentageAlcoholByVolume > gtAlcoholByVolumeDecimal);
                }

                if (!string.IsNullOrWhiteSpace(ltAlcoholByVolume) && decimal.TryParse(ltAlcoholByVolume, out decimal ltAlcoholByVolumeDecimal))
                {
                    query = _context.Beers.Where(x => x.PercentageAlcoholByVolume < ltAlcoholByVolumeDecimal);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllBeers failed.");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Beer>> GetBeerById(int id)
        {
            try
            {
                var beer = await _context.Beers
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (beer == null)
                {
                    return NotFound(id);
                }

                return beer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetBeerById failed.");
                return BadRequest(ex);
            }
        }
    }
}
