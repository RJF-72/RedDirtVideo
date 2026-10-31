using System.ComponentModel.DataAnnotations;

namespace RedDirtVideo.API.DTOs;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponse
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
}

public class CreateArtistRequest
{
    [Required]
    public string StageName { get; set; } = string.Empty;
    public string? Genre { get; set; }
    public string? Bio { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? YoutubeUrl { get; set; }
}

public class UpdateArtistRequest
{
    public string? StageName { get; set; }
    public string? Genre { get; set; }
    public string? Bio { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? YoutubeUrl { get; set; }
}

public class SongUploadRequest
{
    [Required]
    public Guid ArtistId { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Genre { get; set; }
}

public class LyricsUploadRequest
{
    [Required]
    public string Lyrics { get; set; } = string.Empty;
}

public class CreateProjectRequest
{
    [Required]
    public Guid ArtistId { get; set; }

    [Required]
    public Guid SongId { get; set; }

    [Required]
    public string ProjectName { get; set; } = string.Empty;

    public string? VideoStyle { get; set; }
}

public class FullProjectGenerateRequest
{
    [Required]
    public Guid ArtistId { get; set; }

    [Required]
    public Guid SongId { get; set; }

    public Guid? VenueId { get; set; }
    public string Style { get; set; } = "Texas Country";
    public bool GenerateConcert { get; set; } = true;
    public bool GenerateMusicVideo { get; set; } = true;
    public bool GenerateLyricVideo { get; set; } = true;
    public bool GenerateSocialClips { get; set; } = true;
}

public class FullProjectGenerateResponse
{
    public Guid JobId { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<string> EstimatedOutputs { get; set; } = new();
}

public class ArtistPhotoUploadResponse
{
    public Guid PhotoId { get; set; }
    public string Url { get; set; } = string.Empty;
}

public class JobStatusResponse
{
    public Guid JobId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int Progress { get; set; }
}

public class UsageResponse
{
    public int CreditsRemaining { get; set; }
    public int MonthlyCredits { get; set; }
}

public class CheckoutRequest
{
    [Required]
    public string Plan { get; set; } = "PRO";
}

public class GenerateConcertRequest
{
    [Required]
    public Guid ProjectId { get; set; }

    [Required]
    public Guid VenueId { get; set; }

    public string CrowdType { get; set; } = "COUNTRY";
    public int DurationSeconds { get; set; } = 180;
}

public class GenerateMusicVideoRequest
{
    [Required]
    public Guid ProjectId { get; set; }
    public string Style { get; set; } = "Texas Country";
    public string Resolution { get; set; } = "4K";
}

public class GenerateLyricVideoRequest
{
    [Required]
    public Guid ProjectId { get; set; }
}

public class GenerateSocialClipsRequest
{
    [Required]
    public Guid ProjectId { get; set; }
    public List<string> Platforms { get; set; } = new();
}

public class CreateBandRequest
{
    public bool LeadGuitar { get; set; } = true;
    public bool SteelGuitar { get; set; } = true;
    public bool Bass { get; set; } = true;
    public bool Drums { get; set; } = true;
    public bool Fiddle { get; set; } = true;
}

public class CreateArtworkAnimationRequest
{
    [Required]
    public Guid SongId { get; set; }
}
