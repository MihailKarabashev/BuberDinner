using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers
{
    public class ErrorController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            //get error from the exception where you can find the error from your services
            Exception? exception = this.HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            return this.Problem(title: exception?.Message);
        }

    }
}
