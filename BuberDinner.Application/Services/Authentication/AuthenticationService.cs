using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Error;
using BuberDinner.Domain.Entities;
using ErrorOr;

namespace BuberDinner.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly IUserRepository usersRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository usersRepository)
        {
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.usersRepository = usersRepository;
        }

        public ErrorOr<AuthenticationResult> Login(string email, string password)
        {
            if (usersRepository.GetUserByEmail(email) is not User user)
                return Errors.Authentication.InvalidCredentials;

            if (user.Password != password)
                return Errors.Authentication.InvalidCredentials;

            var token = this.jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }

        public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
        {
            if (usersRepository.GetUserByEmail(email) is not null)
                return Errors.User.DuplicateEmail;

            var user = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };

            this.usersRepository.Add(user);

            var token = this.jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
              user,
              token);
        }
    }
}
