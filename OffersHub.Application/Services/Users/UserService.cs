using Mapster;
using Microsoft.Extensions.Options;
using OffersHub.Application.Exceptions.Users;
using OffersHub.Application.Models.Users;
using OffersHub.Application.Repositories;
using OffersHub.Domain.Contracts;
using OffersHub.Domain.Models;
using OffersHub.Domain.Security;

namespace OffersHub.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOptions<JWTConfig> _jwtConfig;
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IOptions<JWTConfig> jwtConfig)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _jwtConfig = jwtConfig;
        }

        public async Task<UserRegisterModel> Authenticate(string username, string password, CancellationToken cancellationToken)
        {
            var hashPassword = PasswordHasher.HashPassword(password);
            var result = await _userRepository.Get(username, hashPassword, cancellationToken).ConfigureAwait(false);

            if (result == null)
                throw new UserDoesNotExist("username or password is incorrect");

            _userRepository.Attach(result);
            
            if (!string.IsNullOrEmpty(result.Token))
            {
                result.Token = null;
                _userRepository.Update(result);  
                await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }

            var token = TokenService.GenerateSecurityToken(result.UserName, result.Role.ToString(), _jwtConfig);
            Console.WriteLine($"Generated Token: {token}");


            result.Token = token;
            _userRepository.Update(result);  
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return result.Adapt<UserRegisterModel>(); 
        }

        public async Task<string> Create(UserRegisterModel userModel, CancellationToken cancellationToken)
        {
            var exists = await _userRepository.Exists(userModel.UserName, cancellationToken).ConfigureAwait(false);

            if (exists)
                throw new UserAlreadyExists("user already exists");
            

            var user = userModel.Adapt<User>();
            user.PasswordHash = PasswordHasher.HashPassword(userModel.Password);
            //user.Token = TokenService.GenerateSecurityToken(user.UserName, user.Role.ToString(), _jwtConfig);
            var result = await _userRepository.Create(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);  
            return result;
        }

        public async Task<UserRegisterModel> Authenticate(string token, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetByToken(token, cancellationToken).ConfigureAwait(false);

            if (result == null)
                throw new TokenGotExpired("your time expired"); 

            return result.Adapt<UserRegisterModel>();
        }

        public async Task<string> GetRole(string username, string password, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Get(username, password, cancellationToken).ConfigureAwait(false);
            if (user != null)
            {
                return user.Role.ToString(); 
            }
            return "Guest";
        }
    }
}
