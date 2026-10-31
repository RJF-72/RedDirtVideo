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
public class BillingController : ControllerBase
{
    private readonly RedDirtVideoDbContext _dbContext;

    public BillingController(RedDirtVideoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("subscription")]
    public async Task<ActionResult<object>> GetCurrentSubscription()
    {
        var userId = GetCurrentUserId();
        var subscription = await _dbContext.Subscriptions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync();

        return Ok(new
        {
            plan = subscription is null ? "FREE" : "PRO",
            creditsRemaining = subscription?.CreditsRemaining ?? 10,
            status = subscription?.Status ?? "ACTIVE"
        });
    }

    [HttpGet("usage")]
    public async Task<ActionResult<UsageResponse>> GetUsage()
    {
        var userId = GetCurrentUserId();
        var subscription = await _dbContext.Subscriptions
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync();

        return Ok(new UsageResponse
        {
            CreditsRemaining = subscription?.CreditsRemaining ?? 10,
            MonthlyCredits = 1000
        });
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> CreateCheckout([FromBody] CheckoutRequest request)
    {
        var userId = GetCurrentUserId();

        var payment = new Payment
        {
            UserId = userId,
            Amount = request.Plan == "PRO" ? 49m : 19m,
            Currency = "USD",
            Provider = "STRIPE",
            ProviderTransactionId = Guid.NewGuid().ToString("N"),
            Status = "PENDING",
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Payments.Add(payment);
        await _dbContext.SaveChangesAsync();

        return Ok(new { paymentId = payment.Id, status = payment.Status, plan = request.Plan });
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim is null ? Guid.Empty : Guid.Parse(claim.Value);
    }
}
