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
public class ProjectsController : ControllerBase
{
    private readonly RedDirtVideoDbContext _dbContext;

    public ProjectsController(RedDirtVideoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<ActionResult<object>> CreateProject([FromBody] CreateProjectRequest request)
    {
        var artist = await _dbContext.Artists.FindAsync(request.ArtistId);
        var song = await _dbContext.Songs.FindAsync(request.SongId);

        if (artist is null || song is null)
        {
            return BadRequest("Artist and Song must both exist.");
        }

        var project = new Project
        {
            UserId = GetCurrentUserId(),
            ArtistId = request.ArtistId,
            SongId = request.SongId,
            Name = request.ProjectName,
            Style = request.VideoStyle ?? "Texas Country",
            Status = "QUEUED",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Projects.Add(project);
        await _dbContext.SaveChangesAsync();

        return Ok(new { projectId = project.Id });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
    {
        var projects = await _dbContext.Projects
            .Where(p => p.UserId == GetCurrentUserId())
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return Ok(projects);
    }

    [HttpGet("{projectId:guid}")]
    public async Task<ActionResult<Project>> GetProject(Guid projectId)
    {
        var project = await _dbContext.Projects.FindAsync(projectId);
        if (project is null)
        {
            return NotFound();
        }

        return Ok(project);
    }

    [HttpDelete("{projectId:guid}")]
    public async Task<IActionResult> DeleteProject(Guid projectId)
    {
        var project = await _dbContext.Projects.FindAsync(projectId);
        if (project is null)
        {
            return NotFound();
        }

        _dbContext.Projects.Remove(project);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{projectId:guid}/storyboard")]
    public async Task<ActionResult<object>> GenerateStoryboard(Guid projectId)
    {
        var project = await _dbContext.Projects.FindAsync(projectId);
        if (project is null)
        {
            return NotFound();
        }

        var storyboard = new Storyboard
        {
            ProjectId = projectId,
            Status = "GENERATING",
            GeneratedPrompt = $"Create a {project.Style ?? "Texas Country"} narrative storyboard for {project.Name}.",
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Storyboards.Add(storyboard);
        await _dbContext.SaveChangesAsync();

        return Ok(new { storyboardId = storyboard.Id, status = "processing" });
    }

    [HttpGet("{projectId:guid}/storyboard")]
    public async Task<ActionResult<object>> GetStoryboard(Guid projectId)
    {
        var storyboard = await _dbContext.Storyboards
            .Where(s => s.ProjectId == projectId)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync();

        if (storyboard is null)
        {
            return NotFound();
        }

        return Ok(storyboard);
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim is null ? Guid.Empty : Guid.Parse(claim.Value);
    }
}
