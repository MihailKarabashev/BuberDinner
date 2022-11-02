using ErrorOr;
using MediatR;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Error;
using BuberDinner.Application.Authentication.Common;

namespace BuberDinner.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly IUserRepository usersRepository;

        public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository usersRepository)
        {
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.usersRepository = usersRepository;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            if (usersRepository.GetUserByEmail(command.Email) is not null)
                return Errors.User.DuplicateEmail;

            var user = new Domain.Entities.User()
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Password = command.Password
            };

            usersRepository.Add(user);

            var token = jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(
              user,
              token);

        }
    }
}
