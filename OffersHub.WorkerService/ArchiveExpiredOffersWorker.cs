using Microsoft.Extensions.DependencyInjection;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Contracts;
using OffersHub.Domain.Models;

namespace OffersHub.WorkerService
{
    public class ArchiveExpiredOffersWorker : BackgroundService
    {
        private readonly ILogger<ArchiveExpiredOffersWorker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        //private readonly IOfferRepository _offerRepository;
        //private readonly IUnitOfWork _unitOfWork;
        public ArchiveExpiredOffersWorker(ILogger<ArchiveExpiredOffersWorker> logger,
            IServiceScopeFactory serviceScopeFactory)
                                          //IOfferRepository offerRepository,
                                          //IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            //_offerRepository = offerRepository;
            //_unitOfWork = unitOfWork;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Log the background task running
                _logger.LogInformation("ArchiveExpiredOffersWorker running at: {time}", DateTimeOffset.Now);

                // Archive expired offers
                await ArchiveExpiredOffersAsync(cancellationToken);

                // Wait for the next interval
                await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            }
        }

        private async Task ArchiveExpiredOffersAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope()) // Create a new scope
            {
                var scopedOfferRepository = scope.ServiceProvider.GetRequiredService<IOfferRepository>();
                var scopedUnitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var expiredOffers = await scopedOfferRepository.GetExpiredOffersAsync(DateTime.UtcNow, cancellationToken);

                if (!expiredOffers.Any())
                {
                    _logger.LogInformation("No expired offers to archive.");
                    return;
                }

                foreach (var offer in expiredOffers)
                {
                    offer.Status = OfferStatus.Archived;
                }

                await scopedUnitOfWork.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Archived {count} expired offers.", expiredOffers.Count);
            } // Scope is automatically disposed
        }
    }
}
