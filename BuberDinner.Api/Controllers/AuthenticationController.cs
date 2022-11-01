using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;
using BuberDinner.Domain.Common.Error;

namespace BuberDinner.Api.Controllers
{
    [Route("auth")]
    public class AuthenticationController : ApiController
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [Route("register")][HttpPost]
        public IActionResult Register(RegisterRequest request)
        {
            var authResult = this.authenticationService.Register(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password);

            return authResult.Match(
                authResult => this.Ok(MapAuthResult(authResult)),
                errors => this.Problem(errors));
        }
       

        [Route("login")] [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            var authResult = this.authenticationService.Login(
                request.Email,
                request.Password);

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
