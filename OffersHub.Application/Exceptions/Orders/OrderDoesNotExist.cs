
namespace OffersHub.Application.Exceptions.Orders
{
    public class OrderDoesNotExist : Exception
    {
        public OrderDoesNotExist(string message) : base(message) { }
    }
}
