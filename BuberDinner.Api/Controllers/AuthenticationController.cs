using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Commands;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Error;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ErrorOr;
using BuberDinner.Application.Authentication.Queries.Login;

namespace BuberDinner.Api.Controllers
{
    [Route("auth")]
    public class AuthenticationController : ApiController
    {
        private readonly ISender mediator;

        public AuthenticationController(ISender mediator)
        {
            this.mediator = mediator;
        }

        [Route("register")][HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = new RegisterCommand(request.FirstName,
                request.LastName,
                request.Email,
                request.Password);

            ErrorOr<AuthenticationResult> authResult = await this.mediator.Send(command);

            return authResult.Match(
                authResult => this.Ok(MapAuthResult(authResult)),
                errors => this.Problem(errors));
        }
       

        [Route("login")] [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var loginQuery = new LoginQuery(request.Email, request.Password);
            var authResult = await this.mediator.Send(loginQuery);

            if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
            {
                return this.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: authResult.FirstError.Description);
            }


            return authResult.Match(
                authResult => this.Ok(MapAuthResult(authResult)),
                errors => this.Problem(errors));
        }


        private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
        {
            return new AuthenticationResponse(
                 authResult.User.Id,
                 authResult.User.FirstName,
                 authResult.User.LastName,
                 authResult.User.Email,
                 authResult.Token
                );
        }
    }
}
