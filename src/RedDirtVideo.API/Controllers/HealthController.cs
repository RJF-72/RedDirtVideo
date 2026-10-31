using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedDirtVideo.API.DTOs;
using RedDirtVideo.Domain.Entities;
using RedDirtVideo.Infrastructure.Data;

namespace RedDirtVideo.API.Controllers;

[ApiController]
[Route("api/v1/")]
[Authorize]
public class GenerateController : ControllerBase
{
    private readonly RedDirtVideoDbContext _dbContext;

    public GenerateController(RedDirtVideoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("generate/full-project")]
    public async Task<ActionResult<FullProjectGenerateResponse>> GenerateFullProject([FromBody] FullProjectGenerateRequest request)
    {
        var artistExists = await _dbContext.Artists.AnyAsync(a => a.Id == request.ArtistId);
        var songExists = await _dbContext.Songs.AnyAsync(s => s.Id == request.SongId);

        if (!artistExists || !songExists)
        {
            return BadRequest("Artist and song must both exist.");
        }

        var job = new AiJob
        {
            UserId = GetCurrentUserId(),
            ProjectId = null,
            JobType = "FULL_PROJECT",
            QueueName = "red-dirt-video",
            Priority = 5,
            Status = "QUEUED",
            Progress = 0,
            StartedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.AiJobs.Add(job);
        await _dbContext.SaveChangesAsync();

        var outputs = new List<string>();
        if (request.GenerateMusicVideo) outputs.Add("Music Video");
        if (request.GenerateLyricVideo) outputs.Add("Lyric Video");
        if (request.GenerateConcert) outputs.Add("Concert Video");
        if (request.GenerateSocialClips) outputs.Add("5 Social Clips");

        return Ok(new FullProjectGenerateResponse
        {
            JobId = job.Id,
            Status = "queued",
            EstimatedOutputs = outputs
        });
    }

    [HttpPost("concerts/generate")]
    public async Task<ActionResult<object>> GenerateConcert([FromBody] GenerateConcertRequest request)
    {
        var project = await _dbContext.Projects.FindAsync(request.ProjectId);
        if (project is null)
        {
            return NotFound();
        }

        var job = new AiJob
        {
            UserId = GetCurrentUserId(),
            ProjectId = request.ProjectId,
            JobType = "VENUE_RENDER",
            QueueName = "concert-generation",
            Priority = 6,
            Status = "QUEUED",
            Progress = 0,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.AiJobs.Add(job);
        await _dbContext.SaveChangesAsync();

        return Ok(new { jobId = job.Id, status = "queued" });
    }

    [HttpPost("videos/music-video")]
    public async Task<ActionResult<object>> GenerateMusicVideo([FromBody] GenerateMusicVideoRequest request)
    {
        var project = await _dbContext.Projects.FindAsync(request.ProjectId);
        if (project is null)
        {
            return NotFound();
        }

        var job = new AiJob
        {
            UserId = GetCurrentUserId(),
            ProjectId = request.ProjectId,
            JobType = "VIDEO_GEN",
            QueueName = "music-video",
            Priority = 4,
            Status = "QUEUED",
            Progress = 0,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.AiJobs.Add(job);
        await _dbContext.SaveChangesAsync();

        return Ok(new { jobId = job.Id });
    }

    [HttpPost("videos/lyric-video")]
    public async Task<ActionResult<object>> GenerateLyricVideo([FromBody] GenerateLyricVideoRequest request)
    {
        var project = await _dbContext.Projects.FindAsync(request.ProjectId);
        if (project is null)
        {
            return NotFound();
        }

        var job = new AiJob
        {
            UserId = GetCurrentUserId(),
            ProjectId = request.ProjectId,
            JobType = "LYRIC_VIDEO",
            QueueName = "lyric-video",
            Priority = 3,
            Status = "QUEUED",
            Progress = 0,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.AiJobs.Add(job);
        await _dbContext.SaveChangesAsync();

        return Ok(new { jobId = job.Id });
    }

    [HttpPost("videos/social-clips")]
    public async Task<ActionResult<object>> GenerateSocialClips([FromBody] GenerateSocialClipsRequest request)
    {
        var project = await _dbContext.Projects.FindAsync(request.ProjectId);
        if (project is null)
        {
            return NotFound();
        }

        var job = new AiJob
        {
            UserId = GetCurrentUserId(),
            ProjectId = request.ProjectId,
            JobType = "SOCIAL_CLIPS",
            QueueName = "social-clips",
            Priority = 3,
            Status = "QUEUED",
            Progress = 0,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.AiJobs.Add(job);
        await _dbContext.SaveChangesAsync();

        return Ok(new { jobId = job.Id, platforms = request.Platforms });
    }

    [HttpPost("projects/{projectId:guid}/band")]
    public async Task<ActionResult<object>> ConfigureBand(Guid projectId, [FromBody] CreateBandRequest request)
    {
        var project = await _dbContext.Projects.FindAsync(projectId);
        if (project is null)
        {
            return NotFound();
        }

        return Ok(new
        {
            projectId,
            leadGuitar = request.LeadGuitar,
            steelGuitar = request.SteelGuitar,
            bass = request.Bass,
            drums = request.Drums,
            fiddle = request.Fiddle,
            status = "configured"
        });
    }

    [HttpPost("cover-art/animate")]
    public async Task<ActionResult<object>> AnimateCoverArt([FromBody] CreateArtworkAnimationRequest request)
    {
        var song = await _dbContext.Songs.FindAsync(request.SongId);
        if (song is null)
        {
            return NotFound();
        }

        return Ok(new { songId = request.SongId, status = "queued", type = "VISUALIZER" });
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim is null ? Guid.Empty : Guid.Parse(claim.Value);
    }
}
