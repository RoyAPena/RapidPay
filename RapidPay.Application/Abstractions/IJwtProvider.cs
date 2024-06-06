namespace RapidPay.Application.Abstractions
{
    public interface IJwtProvider
    {
        public string Generate(string username);
    }
}