﻿using RapidPay.Domain.Abstractions;

namespace RapidPay.Infrastructure.Services
{
    public class PaymentFeeService : IPaymentFeeService
    {
        private decimal currentFee;
        private Random random;

        public PaymentFeeService()
        {
            random = new Random();
            currentFee = (decimal)random.NextDouble() * 2;
            StartFeeUpdate();
        }

        public decimal GetCurrentFee()
        {
            return currentFee;
        }

        private void StartFeeUpdate()
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

        private void UpdateFee()
        {
            var multiplier = (decimal)random.NextDouble();

            currentFee *= multiplier;
        }
    }
}
