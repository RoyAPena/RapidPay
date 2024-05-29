using SharedKernel;

namespace RapidPay.Domain.Users
{
    public static class UserErrors
    {
        public static Error InvalidCredential() => Error.NotFound(
            "User", "Invalid Credential");

        public static Error UsernameShouldBeUnique() => Error.Conflict(
            "User.Username", "The username exists.");
    }
}