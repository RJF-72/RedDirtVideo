using RedDirtVideo.Domain.Entities;

namespace RedDirtVideo.API.Services;

public interface ITokenService
{
    string Generate(User user);
}
