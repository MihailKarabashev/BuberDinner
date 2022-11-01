namespace BuberDinner.Domain.Common.Error
{
    public static partial class Errors
    {
        public static class Authentication
        {
            public static ErrorOr.Error InvalidCredentials =>
                  ErrorOr.Error.Validation(code: "Auth.InvalidCredentials", description: "Invalid Credentials.");
                       
        }
    }
}
