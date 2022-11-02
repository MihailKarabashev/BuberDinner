using BuberDinner.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Commands.Register
{
    // RegisterCommand == RegisterRequest (data what we need)
    public record RegisterCommand(
        string FirstName,
        string LastName,
        string Email,      //this is what returns
        string Password) : IRequest<ErrorOr<AuthenticationResult>>;

    // we specify what to expect as parameters and what to return as response
}
