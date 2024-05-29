using SharedKernel;

namespace RapidPay.Domain.Cards
{
    public static class CardErrors
    {
        public static Error CardNotFound(string cardNumber) => Error.NotFound(
            "Card.CardNumber", $"Card {cardNumber} not found");

        public static Error AmountMoreThan0() => Error.Validation(
            "Amount", "The amount for pay should be more than 0");

        public static Error InsufficientBalance() => Error.Validation(
            "Card.Balance", "Insufficient balance");

        public static Error CardNumberLength() => Error.Validation(
            "Card.CardNumber", "Card number should has 15 digits");

        public static Error CardAlreadyExists() => Error.Validation(
            "Card.CardNumber", "Card number exists");
    }
}