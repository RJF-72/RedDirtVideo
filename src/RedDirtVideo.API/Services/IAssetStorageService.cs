using Microsoft.AspNetCore.Http;

namespace RedDirtVideo.API.Services;

public interface IAssetStorageService
{
    Task<string> SaveAsync(IFormFile file, string? subFolder = null, CancellationToken cancellationToken = default);
    Task DeleteAsync(string storageUrl, CancellationToken cancellationToken = default);
}

public class LocalAssetStorageService : IAssetStorageService
{
    private readonly string _rootPath;

    public LocalAssetStorageService()
    {
        _rootPath = Path.Combine(AppContext.BaseDirectory, "uploads");
        Directory.CreateDirectory(_rootPath);
    }

    public async Task<string> SaveAsync(IFormFile file, string? subFolder = null, CancellationToken cancellationToken = default)
    {
        if (file is null || file.Length == 0)
        {
            throw new ArgumentException("File is empty.", nameof(file));
        }

        var safeFileName = Path.GetFileName(file.FileName);
        var uniqueName = $"{Guid.NewGuid():N}_{safeFileName}";

        var targetFolder = string.IsNullOrWhiteSpace(subFolder)
            ? _rootPath
            : Path.Combine(_rootPath, subFolder);

        Directory.CreateDirectory(targetFolder);

        var targetPath = Path.Combine(targetFolder, uniqueName);

        await using (var stream = new FileStream(targetPath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return $"/uploads/{string.IsNullOrWhiteSpace(subFolder) ? uniqueName : Path.Combine(subFolder, uniqueName).Replace('\\', '/')}";
    }

    public Task DeleteAsync(string storageUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(storageUrl))
        {
            return Task.CompletedTask;
        }

        var relative = storageUrl.TrimStart('/');
        var filePath = Path.Combine(AppContext.BaseDirectory, relative.Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}
