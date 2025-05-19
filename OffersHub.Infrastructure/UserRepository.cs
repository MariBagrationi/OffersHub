using Mapster;
using Microsoft.EntityFrameworkCore;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Models;
using OffersHub.Persistance.Context;

namespace OffersHub.Infrastructure
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(OffersHubContext context) : base(context)
        {
        }

        public async Task<string> Create(User user, CancellationToken cancellationToken)
        {
            await base.CreateAsync(user, cancellationToken).ConfigureAwait(false);
            return user.UserName;
        }

        public async Task<bool> Exists(string username, CancellationToken cancellationToken)
        {
            return await base.AnyAsync(x => x.UserName == username, cancellationToken).ConfigureAwait(false);
        }

        public Task<User?> Get(string username, string passwordHash, CancellationToken cancellationToken)
        {
            var result = Table.AsNoTracking().Where(x => x.UserName == username && x.PasswordHash == passwordHash).FirstOrDefault();
            return Task.FromResult(result);
        }
        public Task<User?> Get(string username, CancellationToken cancellationToken)
        {
            var result = Table.AsNoTracking().Where(x => x.UserName == username).FirstOrDefault();
            return Task.FromResult(result);
        }

        public IQueryable<User> GetAll()
        {
            return Table;
        }

        public async Task<User?> GetByToken(string token, CancellationToken cancellationToken)
        {
            return await Table.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Token == token && x.TokenExpiration > DateTime.UtcNow, cancellationToken)
                .ConfigureAwait(false);
        }

        public new void Update(User user)
        {
            base.Update(user);
        }
    }
}
