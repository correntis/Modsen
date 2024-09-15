
namespace Library.Core.Abstractions
{
    public interface ITokenService
    {
        string CreateAccessToken(Guid userId, List<string> userRoles);
        string CreateRefreshToken();
    }
}