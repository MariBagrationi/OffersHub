
namespace OffersHub.Application.Exceptions.Users
{
    public class TokenGotExpired : Exception
    {
        public TokenGotExpired(string message) : base(message) { }
    }
}
