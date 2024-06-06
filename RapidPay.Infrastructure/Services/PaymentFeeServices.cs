using RapidPay.Domain.Abstractions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RapidPay.Infrastructure.Tests")]
namespace RapidPay.Infrastructure.Services
{
    public class PaymentFeeServices : IPaymentFeeService
    {
        internal decimal currentFee;
        private Random _random;

        public PaymentFeeServices(Random random)
        {
            _random = random;
            currentFee = (decimal)_random.NextDouble() * 2;
            StartFeeUpdate();
        }

        public decimal GetCurrentFee()
        {
            return currentFee;
        }

        internal void StartFeeUpdate()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromHours(1));
                    UpdateFee();
                }
            });
        }

        internal void UpdateFee()
        {
            var multiplier = (decimal)_random.NextDouble() * 2;

            currentFee *= multiplier;
        }
    }
}
