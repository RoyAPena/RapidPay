namespace RapidPay.Application.Abstractions
{
    public interface IUserServices
    {
        Task<string> CreateUserAsync(string userName, string password);
        Task<bool> LoginAsync(string userName, string password);
        Task<bool> UsernameExists(string username);
        Task<bool> ValidatePassword(string password);
    }
}