using OffersHub.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace OffersHub.Application.Repositories
{
    public interface IUserRepository
    {
        IQueryable<User> GetAll();
        Task<string> Create(User user, CancellationToken cancellationToken);

        Task<User?> Get(string username, string passwordHash, CancellationToken cancellationToken);
        Task<User?> Get(string username, CancellationToken cancellationToken);

        Task<bool> Exists(string username, CancellationToken cancellationToken);

        Task<User?> GetByToken(string token, CancellationToken cancellationToken);

        void Update(User user);
        void Attach(User user);
        void Detach(User user);
    }
}
