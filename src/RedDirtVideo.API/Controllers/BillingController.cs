using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedDirtVideo.Domain.Entities;
using RedDirtVideo.Infrastructure.Data;

namespace RedDirtVideo.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class VenuesController : ControllerBase
{
    private readonly RedDirtVideoDbContext _dbContext;

    public VenuesController(RedDirtVideoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Venue>>> GetVenues()
    {
        var venues = await _dbContext.Venues.Where(v => v.Active).OrderBy(v => v.Name).ToListAsync();
        return Ok(venues);
    }

    [HttpGet("{venueId:guid}")]
    public async Task<ActionResult<Venue>> GetVenue(Guid venueId)
    {
        var venue = await _dbContext.Venues.FindAsync(venueId);
        if (venue is null)
        {
            return NotFound();
        }

        return Ok(venue);
    }
}
