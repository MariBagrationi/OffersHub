
namespace OffersHub.Application.Exceptions.Users
{
    public class UserDoesNotExist : Exception
    {
        public UserDoesNotExist(string message) : base(message) { }
    }
}
