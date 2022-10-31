using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;

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

        public AuthenticationResult Login(string email, string password)
        {
            if (usersRepository.GetUserByEmail(email) is not User user)
                //Bad exception message!
                throw new Exception("User with given email doest not exist");

            if (user.Password != password) throw new Exception("Invalid password.");

            var token = this.jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
                user,
                token);
        }

        public AuthenticationResult Register(string firstName, string lastName, string email, string password)
        {
            if (usersRepository.GetUserByEmail(email) is not null)
                throw new Exception("User with given email already exsists.");

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
