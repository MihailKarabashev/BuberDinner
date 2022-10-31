using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
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

            var authResponse = new AuthenticationResponse(
                 authResult.User.Id,
                 authResult.User.FirstName,
                 authResult.User.LastName,
                 authResult.User.Email,
                 authResult.Token
                );

            return Ok(authResponse);
        }

        [Route("login")] [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            var authResult = this.authenticationService.Login(
                request.Email,
                request.Password);

            var authResponse = new AuthenticationResponse(
              authResult.User.Id,
              authResult.User.FirstName,
              authResult.User.LastName,
              authResult.User.Email,
              authResult.Token
             );

            return Ok(authResponse);
        }
    }
}
