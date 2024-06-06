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

        public static Error PasswordIsNotValid = Error.Validation(
            "User.Password", "For a strong password, it's recommended to use a mix of uppercase and lowercase letters, numbers, and symbols. Aim for a length of at least 8 characters.\r\n");

        public static Error UsernameShouldBeUnique = Error.Validation(
            "User.Username", "Username already in use");
    }
}