using BuberDinner.Api.Common.Http;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BuberDinner.Api.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            if (errors.Count is 0)
            {
                return Problem();
            }

            if (errors.All(x => x.Type == ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            this.HttpContext.Items[HttpContextItemKeys.Errors] = errors;

            var firstError = errors[0];
            return Problem(firstError);
        }

        private IActionResult Problem(Error firstError)
        {
            var statusCode = firstError.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            return this.Problem(statusCode: statusCode, title: firstError.Description);
        }

        private IActionResult ValidationProblem(List<Error> errors)
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }

            return ValidationProblem(modelStateDictionary);
        }
    }
}
