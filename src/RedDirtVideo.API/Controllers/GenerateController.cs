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
public class JobsController : ControllerBase
{
    private readonly RedDirtVideoDbContext _dbContext;

    public JobsController(RedDirtVideoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobStatusResponse>>> GetJobs()
    {
        var jobs = await _dbContext.AiJobs
            .Where(j => j.UserId == GetCurrentUserId())
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();

        var response = jobs.Select(j => new JobStatusResponse
        {
            JobId = j.Id,
            Type = j.JobType,
            Status = j.Status,
            Progress = j.Progress
        }).ToList();

        return Ok(response);
    }

    [HttpGet("{jobId:guid}")]
    public async Task<ActionResult<JobStatusResponse>> GetJob(Guid jobId)
    {
        var job = await _dbContext.AiJobs.FindAsync(jobId);
        if (job is null)
        {
            return NotFound();
        }

        return Ok(new JobStatusResponse
        {
            JobId = job.Id,
            Type = job.JobType,
            Status = job.Status,
            Progress = job.Progress
        });
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim is null ? Guid.Empty : Guid.Parse(claim.Value);
    }
}
