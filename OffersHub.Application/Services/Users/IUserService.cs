
using OffersHub.Application.Models.Users;
using OffersHub.Application.Repositories;

namespace OffersHub.Application.Services.Users
{
    public interface IUserService
    {
        Task<UserRegisterModel> Authenticate(string username, string password, CancellationToken cancellationToken);
        Task<UserRegisterModel> Authenticate(string token, CancellationToken cancellationToken);
        Task<string> Create(UserRegisterModel user, CancellationToken cancellationToken);
        Task<string> GetRole(string username, string password, CancellationToken cancellationToken);
    }
}
