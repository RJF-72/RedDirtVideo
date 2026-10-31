using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedDirtVideo.API.DTOs;
using RedDirtVideo.Domain.Entities;
using RedDirtVideo.Infrastructure.Data;

namespace RedDirtVideo.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ArtistsController : ControllerBase
{
    private readonly RedDirtVideoDbContext _dbContext;

    public ArtistsController(RedDirtVideoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Artist>>> GetArtists()
    {
        var artists = await _dbContext.Artists.OrderByDescending(a => a.CreatedAt).ToListAsync();
        return Ok(artists);
    }

    [HttpGet("{artistId:guid}")]
    public async Task<ActionResult<Artist>> GetArtist(Guid artistId)
    {
        var artist = await _dbContext.Artists.FindAsync(artistId);
        if (artist is null)
        {
            return NotFound();
        }

        return Ok(artist);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CreateArtist([FromBody] CreateArtistRequest request)
    {
        var userId = GetCurrentUserId();

        var artist = new Artist
        {
            UserId = userId,
            StageName = request.StageName,
            Genre = request.Genre,
            Bio = request.Bio,
            WebsiteUrl = request.WebsiteUrl,
            InstagramUrl = request.InstagramUrl,
            YoutubeUrl = request.YoutubeUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Artists.Add(artist);
        await _dbContext.SaveChangesAsync();

        return Ok(new { artistId = artist.Id });
    }

    [HttpPut("{artistId:guid}")]
    public async Task<IActionResult> UpdateArtist(Guid artistId, [FromBody] UpdateArtistRequest request)
    {
        var artist = await _dbContext.Artists.FindAsync(artistId);
        if (artist is null)
        {
            return NotFound();
        }

        artist.StageName = request.StageName ?? artist.StageName;
        artist.Genre = request.Genre ?? artist.Genre;
        artist.Bio = request.Bio ?? artist.Bio;
        artist.WebsiteUrl = request.WebsiteUrl ?? artist.WebsiteUrl;
        artist.InstagramUrl = request.InstagramUrl ?? artist.InstagramUrl;
        artist.YoutubeUrl = request.YoutubeUrl ?? artist.YoutubeUrl;
        artist.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{artistId:guid}")]
    public async Task<IActionResult> DeleteArtist(Guid artistId)
    {
        var artist = await _dbContext.Artists.FindAsync(artistId);
        if (artist is null)
        {
            return NotFound();
        }

        _dbContext.Artists.Remove(artist);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{artistId:guid}/photos")]
    public async Task<ActionResult<ArtistPhotoUploadResponse>> UploadPhoto(Guid artistId)
    {
        var artist = await _dbContext.Artists.FindAsync(artistId);
        if (artist is null)
        {
            return NotFound();
        }

        var photo = new Asset
        {
            UserId = GetCurrentUserId(),
            Filename = $"artist-{artistId}-{DateTime.UtcNow:yyyyMMddHHmmss}.jpg",
            FileType = "IMAGE",
            MimeType = "image/jpeg",
            FileSize = 0,
            StorageUrl = $"https://example.com/uploads/{artistId}.jpg",
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Assets.Add(photo);
        await _dbContext.SaveChangesAsync();

        return Ok(new ArtistPhotoUploadResponse
        {
            PhotoId = photo.Id,
            Url = photo.StorageUrl ?? string.Empty
        });
    }

    [HttpGet("{artistId:guid}/photos")]
    public async Task<ActionResult<IEnumerable<Asset>>> GetPhotos(Guid artistId)
    {
        var artistExists = await _dbContext.Artists.AnyAsync(a => a.Id == artistId);
        if (!artistExists)
        {
            return NotFound();
        }

        var assets = await _dbContext.Assets
            .Where(a => a.UserId == GetCurrentUserId())
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return Ok(assets);
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim is null ? Guid.Empty : Guid.Parse(claim.Value);
    }
}
