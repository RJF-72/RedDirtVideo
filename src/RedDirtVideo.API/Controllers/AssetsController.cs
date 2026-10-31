using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RedDirtVideo.API.Services;
using RedDirtVideo.Domain.Entities;
using RedDirtVideo.Infrastructure.Data;

namespace RedDirtVideo.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class AssetsController : ControllerBase
{
    private readonly RedDirtVideoDbContext _dbContext;
    private readonly IAssetStorageService _assetStorageService;

    public AssetsController(RedDirtVideoDbContext dbContext, IAssetStorageService assetStorageService)
    {
        _dbContext = dbContext;
        _assetStorageService = assetStorageService;
    }

    [HttpPost]
    public async Task<ActionResult<object>> UploadAsset(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(new { error = "A valid file is required." });
        }

        var userId = GetCurrentUserId();
        var storageUrl = await _assetStorageService.SaveAsync(file, userId.ToString());

        var asset = new Asset
        {
            UserId = userId,
            Filename = Path.GetFileName(file.FileName),
            FileType = GetFileType(file.ContentType),
            MimeType = file.ContentType,
            FileSize = file.Length,
            StorageUrl = storageUrl,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Assets.Add(asset);
        await _dbContext.SaveChangesAsync();

        return Ok(new
        {
            assetId = asset.Id,
            url = asset.StorageUrl,
            fileName = asset.Filename,
            fileType = asset.FileType
        });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
    {
        var userId = GetCurrentUserId();
        var assets = await _dbContext.Assets
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return Ok(assets);
    }

    [HttpDelete("{assetId:guid}")]
    public async Task<IActionResult> DeleteAsset(Guid assetId)
    {
        var asset = await _dbContext.Assets.FindAsync(assetId);
        if (asset is null)
        {
            return NotFound();
        }

        if (!string.IsNullOrWhiteSpace(asset.StorageUrl))
        {
            await _assetStorageService.DeleteAsync(asset.StorageUrl);
        }

        _dbContext.Assets.Remove(asset);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim is null ? Guid.Empty : Guid.Parse(claim.Value);
    }

    private static string GetFileType(string contentType)
    {
        if (contentType.Contains("image", StringComparison.OrdinalIgnoreCase)) return "IMAGE";
        if (contentType.Contains("audio", StringComparison.OrdinalIgnoreCase)) return "AUDIO";
        if (contentType.Contains("video", StringComparison.OrdinalIgnoreCase)) return "VIDEO";
        return "FILE";
    }
}
