using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Models;
using OffersHub.Persistance.Context;

namespace OffersHub.Infrastructure
{
    public class ClientOfferRepository : BaseRepository<ClientOffer>, IClientOfferRepository
    {
        private new readonly OffersHubContext _context;
        public ClientOfferRepository(OffersHubContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Offer>> GetCartForClientAsync(int clientId, CancellationToken cancellationToken)
        {
            // Writing a raw SQL query to join the ClientOffers with Offer, Category, and Company
            var sqlQuery = @"
            SELECT o.*
            FROM ClientOffers co
            INNER JOIN Offers o ON co.OfferId = o.Id
            INNER JOIN Categories c ON o.CategoryId = c.Id
            INNER JOIN Companies cmp ON o.CompanyId = cmp.Id
            WHERE co.ClientId = @ClientId AND co.IsDeleted = 0
            ";

            var offers = await _context.Offers.FromSqlRaw(sqlQuery, new SqlParameter("@ClientId", clientId))
                                              .AsNoTracking()
                                              .ToListAsync(cancellationToken)
                                              .ConfigureAwait(false);


            return offers;
        }

        public ClientOffer? GetClientOffer(int clientId, int offerId)
        {
            return Table.FirstOrDefault(c => c.ClientId == clientId && c.OfferId == offerId);
        }

        public IQueryable<ClientOffer> GetClientOffers(int clientId, int offerId)
        {
            return Table.Where(c => c.ClientId == clientId && c.OfferId == offerId).AsNoTracking();
        }

        public async Task<List<ClientOffer>> GetCartItemsWithOfferAsync(int clientId, CancellationToken cancellationToken)
        {
            return await _context.ClientOffers
                .Include(co => co.Offer)
                .Where(co => co.ClientId == clientId && !co.IsDeleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<ClientOffer?> GetById(int id, CancellationToken cancellationToken)
        {
            return await base.GetAsync(new object[] { id }, cancellationToken).ConfigureAwait(false);
        }
    }

}
