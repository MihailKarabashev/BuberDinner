using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Commands;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Contracts.Authentication;
using BuberDinner.Domain.Common.Error;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ErrorOr;
using BuberDinner.Application.Authentication.Queries.Login;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;

namespace BuberDinner.Api.Controllers
{
    [Route("auth")]
    [AllowAnonymous]
    public class AuthenticationController : ApiController
    {
        private readonly ISender mediator;
        private readonly IMapper mapper;

        public AuthenticationController(ISender mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [Route("register")][HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = this.mapper.Map<RegisterCommand>(request);

            ErrorOr<AuthenticationResult> authResult = await this.mediator.Send(command);

            return authResult.Match(
                authResult => this.Ok(this.mapper.Map<AuthenticationResponse>(authResult)),
                errors => this.Problem(errors));
        }
       

        [Route("login")] [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            // get Login request (record) and map it to LoginQuery (record)
            var loginQuery = this.mapper.Map<LoginQuery>(request);  

            //send LoginQuery with mediatoR to LoginQueryHandler and return ErrorOr<AuthResult>
            var authResult = await this.mediator.Send(loginQuery);

            if (authResult.IsError && authResult.FirstError == Errors.Authentication.InvalidCredentials)
            {
                return this.Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: authResult.FirstError.Description);
            }


            return authResult.Match(
                //map AuthResult to AuthResponse if all pass
                authResult => this.Ok(this.mapper.Map<AuthenticationResponse>(authResult)),
                errors => this.Problem(errors));
        }
    }
}
