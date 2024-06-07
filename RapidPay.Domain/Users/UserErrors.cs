using SharedKernel;

namespace RapidPay.Domain.Users
{
    public static class UserErrors
    {
        public static Error InvalidCredential = Error.NotFound(
            "User", "Invalid credential");

        public static Error PasswordCannotBeEmpty = Error.Validation(
            "User.Password", "Password cannot be empty.");

        public static Error UsernameCannotBeEmpty = Error.Validation(
            "User.Username", "Username cannot be empty.");
    }
}