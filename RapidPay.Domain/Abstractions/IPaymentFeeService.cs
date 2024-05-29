namespace RapidPay.Domain.Abstractions
{
    public interface IPaymentFeeService
    {
        decimal GetCurrentFee();
    }
}