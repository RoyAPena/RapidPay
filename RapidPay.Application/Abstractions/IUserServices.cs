namespace RapidPay.Application.Abstractions
{
    public interface IUserServices
    {
        Task<bool> LoginAsync(string userName, string password);
    }
}