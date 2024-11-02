using IPF_Beer_API.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPF_Beer_API.Controllers
{
    [ApiController]
    [Route("bar")]
    public class BarController : ControllerBase
    {
        private readonly ILogger<BarController> _logger;

        private readonly Data.DataContext _context;

        public BarController(ILogger<BarController> logger, Data.DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Bar>> AddBar(Bar bar)
        {
            try
            {
                _context.Bars.Add(bar);
                await _context.SaveChangesAsync();

                return bar;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddBar failed.");
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Bar>> UpdateBarById(int id, Bar bar)
        {
            try
            {
                var barToUpdate = await _context.Bars
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (barToUpdate != null)
                {
                    barToUpdate.Name = bar.Name;
                    barToUpdate.Address = bar.Address;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound(bar);
                }

                return bar;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateBarById failed.");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Bar>>> GetAllBars()
        {
            try
            {
                return await _context.Bars.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllBars failed.");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Bar>> GetBarById(int id)
        {
            try
            {
                var bar = await _context.Bars
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (bar == null)
                {
                    return NotFound(bar);
                }

                return bar;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetBarById failed.");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("beer/{beerId}")]
        public async Task<ActionResult<Bar>> AddBarBeerLink(Bar bar, int beerId)
        {
            try
            {
                var barToUpdate = await _context.Bars
                    .SingleOrDefaultAsync(x => x.Id == bar.Id);

                var beerFound = await _context.Beers
                    .SingleOrDefaultAsync(x => x.Id == beerId);

                if (barToUpdate != null && beerFound != null)
                {
                    if (barToUpdate.BeerTypes == null)
                        barToUpdate.BeerTypes = new List<int>();

                    barToUpdate.BeerTypes.Add(beerId);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound(bar);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddBarBeerLink failed.");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{barId}/beer")]
        public async Task<ActionResult<BarsDto>> GetBarWithAssociatedBeersById(int barId)
        {
            try
            {
                var bar = await _context.Bars
                    .SingleOrDefaultAsync(x => x.Id == barId);

                if (bar == null)
                {
                    return NotFound(bar);
                }

                var associatedBeers = new List<Beer>();
                foreach (var beerId in bar.BeerTypes)
                {
                    associatedBeers.Add(await _context.Beers.SingleAsync(x => x.Id == beerId));
                }

                var result = new BarsDto { Bar = bar, AssociatedBeers = associatedBeers };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetBarWithAssociatedBeersById failed.");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("beer")]
        public async Task<ActionResult<List<BarsDto>>> GetAllBarsWithAssociatedBeers()
        {
            try
            {
                var bars = await _context.Bars.ToListAsync();
                var result = new List<BarsDto>();

                foreach (var bar in bars)
                {
                    var associatedBeers = new List<Beer>();
                    foreach (var beerId in bar.BeerTypes)
                    {
                        associatedBeers.Add(await _context.Beers.SingleAsync(x => x.Id == beerId));
                    }
                    result.Add(new BarsDto { Bar = bar, AssociatedBeers = associatedBeers });
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllBarsWithAssociatedBeers failed.");
                return BadRequest(ex);
            }
        }
    }

    public class BarsDto
    {
        public required Bar Bar { get; set; }
        public required List<Beer> AssociatedBeers { get; set; }
    }
}
