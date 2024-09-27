
namespace Library.Core.Abstractions
{
    public interface ITokenService
    {
        string CreateAccessToken(Guid userId, string[] userRoles);
        string CreateRefreshToken();
    }
}