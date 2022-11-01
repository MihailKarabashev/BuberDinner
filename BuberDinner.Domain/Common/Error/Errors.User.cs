using ErrorOr;
namespace BuberDinner.Domain.Common.Error
{
    public static partial class Errors
    {
        public static class User
        {
            public static ErrorOr.Error DuplicateEmail => 
                    ErrorOr.Error.Conflict("User.DuplicateEmail","Email already in use.");
        }
    }
}
