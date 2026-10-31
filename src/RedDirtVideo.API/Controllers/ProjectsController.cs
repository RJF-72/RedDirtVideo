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
public class SongsController : ControllerBase
{
    private readonly RedDirtVideoDbContext _dbContext;

    public SongsController(RedDirtVideoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<ActionResult<object>> UploadSong([FromBody] SongUploadRequest request)
    {
        var artist = await _dbContext.Artists.FindAsync(request.ArtistId);
        if (artist is null)
        {
            return NotFound("Artist not found.");
        }

        var song = new Song
        {
            ArtistId = request.ArtistId,
            Title = request.Title,
            Genre = request.Genre,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Songs.Add(song);
        await _dbContext.SaveChangesAsync();

        return Ok(new { songId = song.Id });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
    {
        var songs = await _dbContext.Songs.OrderByDescending(s => s.CreatedAt).ToListAsync();
        return Ok(songs);
    }

    [HttpGet("{songId:guid}")]
    public async Task<ActionResult<Song>> GetSong(Guid songId)
    {
        var song = await _dbContext.Songs.FindAsync(songId);
        if (song is null)
        {
            return NotFound();
        }

        return Ok(song);
    }

    [HttpPost("{songId:guid}/lyrics")]
    public async Task<IActionResult> UploadLyrics(Guid songId, [FromBody] LyricsUploadRequest request)
    {
        var song = await _dbContext.Songs.FindAsync(songId);
        if (song is null)
        {
            return NotFound();
        }

        var lyric = new Lyric
        {
            SongId = songId,
            LyricsText = request.Lyrics,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Lyrics.Add(lyric);
        await _dbContext.SaveChangesAsync();

        return Ok(new { lyricId = lyric.Id });
    }

    [HttpPost("{songId:guid}/lyrics/analyze")]
    public async Task<ActionResult<object>> AnalyzeLyrics(Guid songId)
    {
        var song = await _dbContext.Songs.FindAsync(songId);
        if (song is null)
        {
            return NotFound();
        }

        var lyric = await _dbContext.Lyrics
            .Where(l => l.SongId == songId)
            .OrderByDescending(l => l.CreatedAt)
            .FirstOrDefaultAsync();

        var sections = new List<object>();
        if (lyric is not null)
        {
            var text = lyric.LyricsText;
            var first = text.Contains("verse", StringComparison.OrdinalIgnoreCase) ? "VERSE" : "INTRO";
            sections.Add(new { type = first, start = 0, end = 30 });
            sections.Add(new { type = "CHORUS", start = 30, end = 60 });
            sections.Add(new { type = "BRIDGE", start = 60, end = 90 });
            sections.Add(new { type = "OUTRO", start = 90, end = 120 });
        }
        else
        {
            sections.Add(new { type = "VERSE", start = 0, end = 30 });
            sections.Add(new { type = "CHORUS", start = 30, end = 60 });
        }

        return Ok(new { sections });
    }
}
