
namespace OffersHub.Application.Exceptions.Categories
{
    public class CategoryAlreadyExists : Exception
    {
        public static string Code { get; private set; } = "Category Already Exists";
        public CategoryAlreadyExists(string message) : base(message) { }
    }
}
