using IPF_Beer_API.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IPF_Beer_API.Controllers
{
    [ApiController]
    [Route("brewery")]
    public class BreweryController : ControllerBase
    {
        private readonly ILogger<BreweryController> _logger;

        private readonly Data.DataContext _context;

        public BreweryController(ILogger<BreweryController> logger, Data.DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Brewery>> AddBrewery(Brewery brewery)
        {
            try
            {
                _context.Breweries.Add(brewery);
                await _context.SaveChangesAsync();

                return brewery;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddBrewery failed.");
                return BadRequest(ex);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Brewery>> UpdateBreweryById(int id, Brewery brewery)
        {
            try
            {
                var breweryToUpdate = await _context.Breweries
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (breweryToUpdate != null)
                {
                    breweryToUpdate.Name = brewery.Name;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound(brewery);
                }

                return brewery;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateBreweryById failed.");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Brewery>>> GetAllBreweries()
        {
            try
            {
                return await _context.Breweries.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllBreweries failed.");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Brewery>> GetBreweryById(int id)
        {
            try
            {
                var brewery = await _context.Breweries
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (brewery == null)
                {
                    return NotFound(brewery);
                }

                return brewery;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetBreweryById failed.");
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("beer/{beerId}")]
        public async Task<ActionResult<Brewery>> AddBreweryBeerLink(Brewery brewery, int beerId)
        {
            try
            {
                var breweryToUpdate = await _context.Breweries
                    .SingleOrDefaultAsync(x => x.Id == brewery.Id);

                var beerFound = await _context.Beers
                    .SingleOrDefaultAsync(x => x.Id == beerId);

                if (breweryToUpdate != null && beerFound != null)
                {
                    if (breweryToUpdate.BeerTypes == null)
                        breweryToUpdate.BeerTypes = new List<int>();

                    breweryToUpdate.BeerTypes.Add(beerId);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return NotFound(brewery);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddBreweryBeerLink failed.");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{breweryId}/beer")]
        public async Task<ActionResult<BreweryDto>> GetBreweryWithAssociatedBeersById(int breweryId)
        {
            try
            {
                var brewery = await _context.Breweries
                    .SingleOrDefaultAsync(x => x.Id == breweryId);

                if (brewery == null)
                {
                    return NotFound(brewery);
                }

                var associatedBeers = new List<Beer>();
                foreach (var beerId in brewery.BeerTypes)
                {
                    associatedBeers.Add(await _context.Beers.SingleAsync(x => x.Id == beerId));
                }

                var result = new BreweryDto { Brewery = brewery, AssociatedBeers = associatedBeers };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetBreweryWithAssociatedBeersById failed.");
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("beer")]
        public async Task<ActionResult<List<BreweryDto>>> GetAllBreweriesWithAssociatedBeers()
        {
            try
            {
                var breweries = await _context.Breweries.ToListAsync();
                var result = new List<BreweryDto>();

                foreach (var brewery in breweries)
                {
                    var associatedBeers = new List<Beer>();
                    foreach (var beerId in brewery.BeerTypes)
                    {
                        associatedBeers.Add(await _context.Beers.SingleAsync(x => x.Id == beerId));
                    }
                    result.Add(new BreweryDto { Brewery = brewery, AssociatedBeers = associatedBeers });
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllBreweriesWithAssociatedBeers failed.");
                return BadRequest(ex);
            }
        }
    }

    public class BreweryDto
    {
        public required Brewery Brewery { get; set; }
        public required List<Beer> AssociatedBeers { get; set; }
    }
}
