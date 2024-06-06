using NSubstitute;
using NUnit.Framework;
using RapidPay.Infrastructure.Services;

namespace RapidPay.Infrastructure.Tests.Services
{
    [TestFixture]
    internal class PaymentFeeServiceTest
    {
        private PaymentFeeServices _feeService;
        private Random SubstituteRandom;

        public PaymentFeeServiceTest()
        {
            SubstituteRandom = Substitute.For<Random>();
            SubstituteRandom.NextDouble().Returns(0.5);
            _feeService = new PaymentFeeServices(SubstituteRandom); // Inject the substitute Random
        }

        [Test]
        public void GetCurrentFee_Should_Return_Initial_Fee()
        {
            // Arrange
            decimal expectedFee = 1.0m; // Assuming initial fee is set to 0.5 from random number generation

            // Act
            var actualFee = _feeService.GetCurrentFee();

            // Assert
            Assert.That(expectedFee, Is.EqualTo(actualFee));
        }

        [Test]
        public void UpdateFee_Should_Multiply_CurrentFee_With_Mocked_Random()
        {
            // Arrange
            decimal initialFee = 0.5m;
            decimal expectedMultiplier = 1.0m;
            decimal expectedUpdatedFee = initialFee * expectedMultiplier;

            _feeService.currentFee = initialFee; // Set initial fee (avoid relying on randomness)

            // Act
            _feeService.UpdateFee();

            // Assert
            // While we cannot directly test the background thread,
            // we can verify that the UpdateFee method updates the currentFee correctly.
            Assert.That(expectedUpdatedFee, Is.EqualTo(_feeService.currentFee));
        }
    }
}