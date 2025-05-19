using FluentValidation;
using OffersHub.Application.Models.Users;

namespace OffersHub.API.Infrastructure.Validators.UserValidators
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterModel>
    {
        public UserRegisterValidator()
        {
            
        }
    }
}
