using RapidPay.Domain.User;

namespace RapidPay.Application.Abstractions
{
    public interface IJwtProvider
    {
        public string Generate(User user);
        public bool ValidateToken(string token);
    }
}