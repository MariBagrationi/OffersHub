
namespace OffersHub.Application.Exceptions.Offers
{
    public class OfferAlreadyExists : Exception
    {
        public OfferAlreadyExists(string message) : base(message) { }
    }
}
