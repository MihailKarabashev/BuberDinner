using ErrorOr;
using FluentValidation;
using MediatR;

namespace BuberDinner.Application.Common.Behaviors
{
    //accepts Request wich should implement IRequest<TResponse> (IRequest come form MediatR, TReponse is your response)
    //returns TResponse => in our case it is something wich implement IErrorOR
   
    public class ValidationBehavior<TRequest,TResponse>
         : IPipelineBehavior<TRequest,TResponse>
          where TRequest : IRequest<TResponse>
          where TResponse : IErrorOr
    {
        private readonly IValidator<TRequest>? validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            this.validator = validator;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (this.validator is null) return await next();

            var validationResult = await this.validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid) return await next();

            var errors = validationResult.Errors
                .ConvertAll(validationFailure => Error.Validation(
                   validationFailure.PropertyName,
                   validationFailure.ErrorMessage));

            return (dynamic)errors;
        }
    }
}
//this method is going to be called before our request goes to the registerCommandHandler

//public class ValidateRegisterCommandBehavior
     //: IPiplineBehavior<RegisterCommand, ErrorOr<AuthenticationResult>>
